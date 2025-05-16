-- Create user with password
CREATE USER azero WITH PASSWORD '9b5f1295-7ed2-422f-9b30-6b0b1d513c37';

-- Create database
CREATE DATABASE "QuizAppDatabase" OWNER postgres;

-- Connect to the new database
\c "QuizAppDatabase"

-- Grant access to the schema
GRANT USAGE ON SCHEMA public TO azero;
GRANT CREATE ON SCHEMA public TO azero;

-- Grant data-level permissions
GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO azero;
GRANT USAGE, SELECT, UPDATE ON ALL SEQUENCES IN SCHEMA public TO azero;

-- Grant permissions on objects created in the future
ALTER DEFAULT PRIVILEGES IN SCHEMA public
GRANT SELECT, INSERT, UPDATE, DELETE ON TABLES TO azero;

ALTER DEFAULT PRIVILEGES IN SCHEMA public
GRANT USAGE, SELECT, UPDATE ON SEQUENCES TO azero;
