CREATE DATABASE SnakeGame;
USE SnakeGame;
CREATE TABLE Game (
    id INT PRIMARY KEY IDENTITY(1,1),
    user_name VARCHAR(100) NOT NULL,
    pass_word INT,
	best_score BIGINT CHECK (best_score >= 0) DEFAULT 0
);
SELECT * FROM Game
UPDATE Game SET best_score = 3 WHERE user_name = 'Hoang'