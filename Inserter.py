from pymongo import MongoClient
import json

# Connect to MongoDB
client = MongoClient("mongodb://127.0.0.1:27017")  # Adjust the connection string if needed
db = client["ExamQuestionsDatabase"]
collection = db["Exams"]

# Load the JSON file
with open(r"E:\QuizAppProject\test.json",encoding = "utf-8") as file:
    json_content = json.load(file)

# Insert the JSON file content as a value to a specific key
document = {
    "Name": "Equillibrium - AzeroFormat`",
    "Questions": json_content
}

collection.insert_one(document)
print("Data inserted successfully!")
