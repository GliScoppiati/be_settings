import os
import uuid
import json
import random
from datetime import datetime, timedelta
from faker import Faker

fake = Faker()

# === Configurazione modificabile ===
N_USERS = 10
N_INGREDIENTS = 25
N_COCKTAILS = 15
N_SUBMISSIONS = 8
N_FAVORITES = 20
N_SEARCHES = 15
MAX_INGREDIENTS_PER_COCKTAIL = 5
APPROVED_SUBMISSIONS_RATIO = 0.5  # % delle submission che vengono approvate come cocktail

# === Directory output ===
OUTPUT_DIR = "output"
os.makedirs(OUTPUT_DIR, exist_ok=True)

# === Generazione Users & Profiles ===
users = []
profiles = []
user_ids = []

for _ in range(N_USERS):
    user_id = str(uuid.uuid4())
    user_ids.append(user_id)
    users.append({
        "Id": user_id,
        "Username": fake.user_name(),
        "Email": fake.email(),
        "PasswordHash": fake.sha256(),
        "CreatedAt": fake.date_time_this_decade().isoformat(),
        "LastLogin": fake.date_time_this_year().isoformat(),
        "IsAdmin": fake.boolean(chance_of_getting_true=10),
        "IsDeleted": False
    })
    profiles.append({
        "UserId": user_id,
        "FirstName": fake.first_name(),
        "LastName": fake.last_name(),
        "BirthDate": fake.date_of_birth(minimum_age=18, maximum_age=60).isoformat(),
        "AlcoholAllowed": True,
        "ConsentGdpr": True,
        "ConsentProfiling": fake.boolean(chance_of_getting_true=70)
    })

# === Generazione Ingredienti ===
ingredients = []
ingredient_ids = []

for _ in range(N_INGREDIENTS):
    ingredient_id = str(uuid.uuid4())
    ingredient_ids.append(ingredient_id)
    name = fake.unique.word().capitalize()
    ingredients.append({
        "IngredientId": ingredient_id,
        "Name": name,
        "NormalizedName": name.lower(),
        "Description": fake.sentence(),
        "Type": random.choice(["liquor", "juice", "spice", "garnish"]),
        "IsAlcoholic": fake.boolean(),
        "Abv": round(random.uniform(0, 50), 2)
    })

# === Generazione Cocktail e CocktailIngredients ===
cocktails = []
cocktail_ingredients = []

for _ in range(N_COCKTAILS):
    cocktail_id = str(uuid.uuid4())
    creator = random.choice(user_ids)
    cocktail = {
        "CocktailId": cocktail_id,
        "OrigId": None,
        "Name": fake.unique.first_name() + " Special",
        "Instructions": fake.paragraph(nb_sentences=2),
        "Category": random.choice(["Classic", "Modern", "Tiki", "Mocktail"]),
        "Glass": random.choice(["Highball", "Martini", "Rocks", "Coupe"]),
        "IsAlcoholic": fake.boolean(chance_of_getting_true=80),
        "ImageUrl": fake.image_url(),
        "SourceType": 1,
        "CreatedAt": fake.date_time_this_year().isoformat(),
        "CreatedBy": creator
    }
    cocktails.append(cocktail)

    for _ in range(random.randint(1, MAX_INGREDIENTS_PER_COCKTAIL)):
        cocktail_ingredients.append({
            "CocktailIngredientId": str(uuid.uuid4()),
            "CocktailId": cocktail_id,
            "IngredientId": random.choice(ingredient_ids),
            "OriginalMeasure": fake.random_element(elements=("1 oz", "2 tsp", "1/2 cup")),
            "QuantityUnit": "ml",
            "QuantityValue": round(random.uniform(10, 60), 2)
        })

# === Generazione Submission e SubmissionIngredients ===
submissions = []
submission_ingredients = []

for _ in range(N_SUBMISSIONS):
    submission_id = str(uuid.uuid4())
    user_id = random.choice(user_ids)
    name = fake.first_name() + " Submission"
    submissions.append({
        "SubmissionId": submission_id,
        "UserId": user_id,
        "Name": name,
        "Instructions": fake.paragraph(),
        "Status": 1,
        "CreatedAt": datetime.utcnow().isoformat(),
        "ImageUrl": fake.image_url(),
        "Category": "Original",
        "Glass": "Rocks",
        "IsAlcoholic": True,
        "Username": fake.user_name()
    })

    for _ in range(random.randint(1, MAX_INGREDIENTS_PER_COCKTAIL)):
        submission_ingredients.append({
            "SubmissionIngredientId": str(uuid.uuid4()),
            "SubmissionId": submission_id,
            "IngredientId": random.choice(ingredient_ids),
            "ProposedName": None,
            "QuantityValue": round(random.uniform(5, 50), 2),
            "QuantityUnit": "ml",
            "OriginalMeasure": "1 oz"
        })

# Alcune submission approvate → aggiunte a Cocktails
approved = random.sample(submissions, int(N_SUBMISSIONS * APPROVED_SUBMISSIONS_RATIO))
for s in approved:
    cocktails.append({
        "CocktailId": str(uuid.uuid4()),
        "OrigId": None,
        "Name": s["Name"],  # stesso nome
        "Instructions": s["Instructions"],
        "Category": s["Category"],
        "Glass": s["Glass"],
        "IsAlcoholic": s["IsAlcoholic"],
        "ImageUrl": s["ImageUrl"],
        "SourceType": 2,
        "CreatedAt": datetime.utcnow().isoformat(),
        "CreatedBy": s["UserId"]
    })

# === Preferiti e Storico Ricerche ===
favorites = []
for _ in range(N_FAVORITES):
    favorites.append({
        "Id": str(uuid.uuid4()),
        "UserId": random.choice(user_ids),
        "CocktailId": random.choice([c["CocktailId"] for c in cocktails]),
        "FavoritedAt": datetime.utcnow().isoformat()
    })

search_histories = []
search_filters = []
for _ in range(N_SEARCHES):
    search_id = str(uuid.uuid4())
    user = random.choice(user_ids)
    search_histories.append({
        "Id": search_id,
        "UserId": user,
        "SearchedAt": datetime.utcnow().isoformat(),
        "Action": "search_cocktail"
    })
    search_filters.append({
        "Id": str(uuid.uuid4()),
        "SearchHistoryId": search_id,
        "FilterType": "category",
        "FilterName": random.choice(["Classic", "Tiki", "Mocktail"])
    })

# === Salvataggio JSON ===
def save_json(filename, data):
    with open(os.path.join(OUTPUT_DIR, filename), "w") as f:
        json.dump(data, f, indent=2)

save_json("users.json", users)
save_json("user_profiles.json", profiles)
save_json("ingredients.json", ingredients)
save_json("cocktails.json", cocktails)
save_json("cocktail_ingredients.json", cocktail_ingredients)
save_json("user_submissions.json", submissions)
save_json("submission_ingredients.json", submission_ingredients)
save_json("favorite_cocktails.json", favorites)
save_json("search_histories.json", search_histories)
save_json("search_filters.json", search_filters)
