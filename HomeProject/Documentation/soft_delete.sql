/*
===============Attempt #1===============

Description:
Use 'master_id' as pointing for old records to current one.
Records have 'dat' indicates date of deletion (if record is not deleted, it is null
'cat' is date of record creation (not equal for example registration date or etc)

ON_UPDATE:
Insert a copy of record, with new '*_id' and correct 'dat'
Childs not affected

ON_DELETE:
Mark a record with correct 'dat'
Mark all childs 'dat' with correct value

Wiew history:
Use correct select sentence to view full history
*/

/*
-- initial
-- database creation
IF db_id('softdelete') IS NOT NULL BEGIN
    USE master
    DROP DATABASE "softdelete"
END
GO

CREATE DATABASE "softdelete"
GO

USE "softdelete"
GO

--create tables
CREATE TABLE Person (
    person_id   INT             NOT NULL    PRIMARY KEY,
    name		VARCHAR(128)    NOT NULL,
	master_id	INT				NOT NULL,
    cat         DATETIME2       NOT NULL,
    dat         DATETIME2       NULL
)

CREATE TABLE Photo (
    photo_id    INT             NOT NULL    PRIMARY KEY,
    person_id   INT             NOT NULL    UNIQUE,
    value       VARCHAR(128)    NOT NULL,
    master_id	INT				NOT NULL,
    cat         DATETIME2       NOT NULL,
    dat         DATETIME2       NULL
)

CREATE TABLE Post (
    post_id     INT             NOT NULL    PRIMARY KEY,
    person_id   INT             NOT NULL,
    title       VARCHAR(128)    NOT NULL,
    description VARCHAR(128)    NOT NULL,
    master_id	INT				NOT NULL,
    cat         DATETIME2       NOT NULL,
    dat         DATETIME2       NULL
)

--add time variables
DECLARE @Time1 DATETIME2
DECLARE @Time2 DATETIME2
DECLARE @Time3 DATETIME2

SET @Time1 = '2020-03-07'
SET @Time2 = '2020-04-10'
SET @Time3 = '2020-05-13'

--database is empty
SELECT 'Database is empty'
SELECT * from Person P 
LEFT JOIN Post Po ON P.person_id = Po.person_id 
LEFT JOIN Photo Ph ON P.person_id = Ph.person_id

--Current time is @Time1
SELECT 'Current time:', @Time1

--insert data
INSERT INTO Person(person_id, name, master_id, cat, dat) VALUES (1, 'Alex', 1, @Time1, null)
INSERT INTO Person(person_id, name, master_id, cat, dat) VALUES (2, 'Valya', 2, @Time1, null)

INSERT INTO Photo (photo_id, person_id, [value], master_id, cat, dat)
VALUES (1, 1, 'Alex_Photo', 1, @Time1, null)
INSERT INTO Photo (photo_id, person_id, [value], master_id, cat, dat)
VALUES (2, 2, 'Valya_Photo', 2, @Time1, null)

INSERT INTO Post (post_id, person_id, title, [description], master_id, cat, dat)
VALUES (1, 1, 'hello world!', 'hello', 1, @Time1, null)

INSERT INTO Post (post_id, person_id, title, [description], master_id, cat, dat)
VALUES (2, 2, 'test1', '', 2, @Time1, null)
INSERT INTO Post (post_id, person_id, title, [description], master_id, cat, dat)
VALUES (3, 2, 'test2', '', 3, @Time1, null)
INSERT INTO Post (post_id, person_id, title, [description], master_id, cat, dat)
VALUES (4, 2, 'test3', '', 4, @Time1, null)

--initial data
SELECT 'Init data'
SELECT * from Person P 
LEFT JOIN Post Po ON P.person_id = Po.person_id 
LEFT JOIN Photo Ph ON P.person_id = Ph.person_id

--Current time is @Time2
SELECT 'Current time:', @Time2

--soft delete Valya's post with value 'test3'
SELECT 'Soft delete Valya`s post with value `test3`'
UPDATE Post SET dat = @Time2 WHERE title LIKE 'test2'
SELECT * from Post

--soft delete Alex (with marking depended records as deleted)
SELECT 'Soft delete Alex'

DECLARE @Alex_id INT
SET @Alex_id = 1

UPDATE Person SET dat = @Time2 WHERE person_id = @Alex_id
UPDATE Post SET dat = @Time2 WHERE person_id = @Alex_id
UPDATE Photo SET dat = @Time2 WHERE person_id = @Alex_id

SELECT * from Person P 
LEFT JOIN Post Po ON P.person_id = Po.person_id 
LEFT JOIN Photo Ph ON P.person_id = Ph.person_id
WHERE P.person_id = @Alex_id

--Current time is @Time3
SELECT 'Current time:', @Time3

--soft update Valya (set name to Valentina) (childs not affected)
SELECT 'Soft delete Valya'
DECLARE @Valya_id INT
SET @Valya_id = 2

--create copy of current record
INSERT INTO Person(person_id, name, master_id, cat, dat) 
SELECT 3, name, @Valya_id, cat, @Time3 FROM Person WHERE person_id = @Valya_id

--modify curent record
UPDATE Person SET name = 'Valentina', cat = @Time3 WHERE person_id = @Valya_id

SELECT 'Valentina`s history'
SELECT * from Person P WHERE master_id = @Valya_id

SELECT 'Current active database records'
SELECT * from Person P 
LEFT JOIN Post Po ON P.person_id = Po.person_id 
LEFT JOIN Photo Ph ON P.person_id = Ph.person_id
WHERE P.dat IS NULL AND Po.dat = P.dat AND Ph.dat = P.dat

--select database records that is valid at @Time2
SELECT 'Database Person records that is valid at @Time2'
SELECT * from Person 
WHERE   (dat > @Time2 or dat is null) 
AND     cat <= @Time2

SELECT 'Database Post records that is valid at @Time2'
SELECT * from Post
WHERE   (dat > @Time2 or dat is null) 
AND     cat <= @Time2

SELECT 'Database Photo records that is valid at @Time2'
SELECT * from Photo
WHERE   (dat > @Time2 or dat is null) 
AND     cat <= @Time2
*/

/*
===============END of Attempt #1===============
*/

/*
===============Attempt #2===============

Description:
Use '*_id' and 'dat' as composite Primary Key
Records have 'dat' indicates date of deletion (if record is not deleted, it is '3000-01-01')
'cat' is date of record creation (not equal for example registration date or etc)

ON_UPDATE:
Insert a copy of record, with new '*_id' and correct 'dat'
Childs not affected

ON_DELETE:
Mark a record with correct 'dat'
Mark all childs 'dat' and composite FK with correct value

Wiew history:
Use correct select sentence to view full history
*/

/*
-- initial
-- database creation
IF db_id('softdelete') IS NOT NULL BEGIN
    USE master
    DROP DATABASE "softdelete"
END
GO

CREATE DATABASE "softdelete"
GO

USE "softdelete"
GO

--create tables
CREATE TABLE Person (
    person_id   INT             NOT NULL,
    dat         DATETIME2       NOT NULL,
    name		VARCHAR(128)    NOT NULL,
    cat         DATETIME2       NOT NULL,
    PRIMARY KEY (person_id, dat)
)

CREATE TABLE Photo (
    photo_id    INT             NOT NULL,
    dat         DATETIME2       NOT NULL,
    person_id   INT             NOT NULL    UNIQUE,
    person_dat  DATETIME2       NOT NULL,
    value       VARCHAR(128)    NOT NULL,
    cat         DATETIME2       NOT NULL,
    PRIMARY KEY (photo_id, dat)
)

CREATE TABLE Post (
    post_id     INT             NOT NULL,
    dat         DATETIME2       NOT NULL,
    person_id   INT             NOT NULL,
    person_dat  DATETIME2       NOT NULL,
    title       VARCHAR(128)    NOT NULL,
    description VARCHAR(128)    NOT NULL,
    cat         DATETIME2       NOT NULL,
    PRIMARY KEY (post_id, dat)
)

--add time variables
DECLARE @Time1 DATETIME2
DECLARE @Time2 DATETIME2
DECLARE @Time3 DATETIME2

SET @Time1 = '2020-03-07'
SET @Time2 = '2020-04-10'
SET @Time3 = '2020-05-13'

--database is empty
SELECT 'Database is empty'
SELECT * from Person P 
LEFT JOIN Post Po ON P.person_id = Po.person_id 
LEFT JOIN Photo Ph ON P.person_id = Ph.person_id

--Current time is @Time1
SELECT 'Current time:', @Time1

--insert data
INSERT INTO Person(person_id, name, cat, dat) VALUES (1, 'Alex', @Time1, '3000-01-01')
INSERT INTO Person(person_id, name, cat, dat) VALUES (2, 'Valya', @Time1, '3000-01-01')

INSERT INTO Photo (photo_id, person_id, person_dat, [value], cat, dat)
VALUES (1, 1, '3000-01-01', 'Alex_Photo', @Time1, '3000-01-01')
INSERT INTO Photo (photo_id, person_id, person_dat, [value], cat, dat)
VALUES (2, 2, '3000-01-01', 'Valya_Photo', @Time1, '3000-01-01')

INSERT INTO Post (post_id, person_id, person_dat, title, [description], cat, dat)
VALUES (1, 1, '3000-01-01', 'hello world!', 'hello', @Time1, '3000-01-01')

INSERT INTO Post (post_id, person_id, person_dat, title, [description], cat, dat)
VALUES (2, 2, '3000-01-01', 'test1', '', @Time1, '3000-01-01')
INSERT INTO Post (post_id, person_id, person_dat, title, [description], cat, dat)
VALUES (3, 2, '3000-01-01', 'test2', '', @Time1, '3000-01-01')
INSERT INTO Post (post_id, person_id, person_dat, title, [description], cat, dat)
VALUES (4, 2, '3000-01-01', 'test3', '', @Time1, '3000-01-01')

--initial data
SELECT 'Init data'
SELECT * from Person P 
LEFT JOIN Post Po ON P.person_id = Po.person_id 
LEFT JOIN Photo Ph ON P.person_id = Ph.person_id

--Current time is @Time2
SELECT 'Current time:', @Time2

--soft delete Valya's post with value 'test3'
SELECT 'Soft delete Valya`s post with value `test3`'
UPDATE Post SET dat = @Time2 WHERE title LIKE 'test2'
SELECT * from Post

--soft delete Alex (with marking depended records as deleted)
SELECT 'Soft delete Alex'

DECLARE @Alex_id INT
DECLARE @Alex_dat DATETIME2
SET @Alex_id = 1
SET @Alex_dat = '3000-01-01'

UPDATE Person SET dat = @Time2 WHERE person_id = @Alex_id AND dat = @Alex_dat
UPDATE Post SET dat = @Time2, person_dat = @Time2 WHERE person_id = @Alex_id AND person_dat = @Alex_dat
UPDATE Photo SET dat = @Time2, person_dat = @Time2 WHERE person_id = @Alex_id AND person_dat = @Alex_dat

SELECT * from Person P 
LEFT JOIN Post Po ON P.person_id = Po.person_id 
LEFT JOIN Photo Ph ON P.person_id = Ph.person_id
WHERE P.person_id = @Alex_id

--Current time is @Time3
SELECT 'Current time:', @Time3

--soft update Valya (set name to Valentina) (childs not affected)
SELECT 'Soft delete Valya'
DECLARE @Valya_id INT
DECLARE @Valya_dat DATETIME2
SET @Valya_id = 2
SET @Valya_dat = '3000-01-01'

--create copy of current record
INSERT INTO Person(person_id, name, cat, dat) 
SELECT @Valya_id, name, cat, @Time3 FROM Person WHERE person_id = @Valya_id AND dat = @Valya_dat

--modify curent record
UPDATE Person SET name = 'Valentina', cat = @Time3 WHERE person_id = @Valya_id AND dat = @Valya_dat

SELECT 'Valentina`s history'
SELECT * from Person WHERE person_id = @Valya_id

SELECT 'Current active database records'
SELECT * from Person P 
LEFT JOIN Post Po ON P.person_id = Po.person_id 
LEFT JOIN Photo Ph ON P.person_id = Ph.person_id
WHERE P.dat = '3000-01-01' AND Po.dat = P.dat AND Ph.dat = P.dat

--select database records that is valid at @Time2
SELECT 'Database Person records that is valid at @Time2'
SELECT * from Person 
WHERE   (dat > @Time2 or dat = '3000-01-01')
AND     cat <= @Time2

SELECT 'Database Post records that is valid at @Time2'
SELECT * from Post
WHERE   (dat > @Time2 or dat = '3000-01-01')
AND     cat <= @Time2

SELECT 'Database Photo records that is valid at @Time2'
SELECT * from Photo
WHERE   (dat > @Time2 or dat = '3000-01-01')
AND     cat <= @Time2
*/

/*
===============END of Attempt #2===============
*/

/*
===============Attempt #3===============

Description:
Use '*_id' and 'dat' as composite Primary Key
Records have 'dat' indicates date of deletion (if record is not deleted, it is '3000-01-01')
'cat' is date of record creation (not equal for example registration date or etc)

ON_UPDATE:
Insert a copy of record, with new '*_id' and correct 'dat'
Childs are soft updated

ON_DELETE:
Mark a record with correct 'dat'
Mark all childs 'dat' and composite FK with correct value

Wiew history:
Use correct select sentence to view full history
*/

-- /*
-- initial
-- database creation
IF db_id('softdelete') IS NOT NULL BEGIN
    USE master
    DROP DATABASE "softdelete"
END
GO

CREATE DATABASE "softdelete"
GO

USE "softdelete"
GO

--create tables
CREATE TABLE Person (
    person_id   INT             NOT NULL,
    dat         DATETIME2       NOT NULL,
    name		VARCHAR(128)    NOT NULL,
    cat         DATETIME2       NOT NULL,
    PRIMARY KEY (person_id, dat)
)

CREATE TABLE Photo (
    photo_id    INT             NOT NULL,
    dat         DATETIME2       NOT NULL,
    person_id   INT             NOT NULL,
    person_dat  DATETIME2       NOT NULL,
    value       VARCHAR(128)    NOT NULL,
    cat         DATETIME2       NOT NULL,
    PRIMARY KEY (photo_id, dat),
    UNIQUE (person_id, person_dat)
)

CREATE TABLE Post (
    post_id     INT             NOT NULL,
    dat         DATETIME2       NOT NULL,
    person_id   INT             NOT NULL,
    person_dat  DATETIME2       NOT NULL,
    title       VARCHAR(128)    NOT NULL,
    description VARCHAR(128)    NOT NULL,
    cat         DATETIME2       NOT NULL,
    PRIMARY KEY (post_id, dat)
)

--add time variables
DECLARE @Time1 DATETIME2
DECLARE @Time2 DATETIME2
DECLARE @Time3 DATETIME2

SET @Time1 = '2020-03-07'
SET @Time2 = '2020-04-10'
SET @Time3 = '2020-05-13'

--database is empty
SELECT 'Database is empty'
SELECT * from Person P 
LEFT JOIN Post Po ON P.person_id = Po.person_id 
LEFT JOIN Photo Ph ON P.person_id = Ph.person_id

--Current time is @Time1
SELECT 'Current time:', @Time1

--insert data
INSERT INTO Person(person_id, name, cat, dat) VALUES (1, 'Alex', @Time1, '3000-01-01')
INSERT INTO Person(person_id, name, cat, dat) VALUES (2, 'Valya', @Time1, '3000-01-01')

INSERT INTO Photo (photo_id, person_id, person_dat, [value], cat, dat)
VALUES (1, 1, '3000-01-01', 'Alex_Photo', @Time1, '3000-01-01')
INSERT INTO Photo (photo_id, person_id, person_dat, [value], cat, dat)
VALUES (2, 2, '3000-01-01', 'Valya_Photo', @Time1, '3000-01-01')

INSERT INTO Post (post_id, person_id, person_dat, title, [description], cat, dat)
VALUES (1, 1, '3000-01-01', 'hello world!', 'hello', @Time1, '3000-01-01')

INSERT INTO Post (post_id, person_id, person_dat, title, [description], cat, dat)
VALUES (2, 2, '3000-01-01', 'test1', '', @Time1, '3000-01-01')
INSERT INTO Post (post_id, person_id, person_dat, title, [description], cat, dat)
VALUES (3, 2, '3000-01-01', 'test2', '', @Time1, '3000-01-01')
INSERT INTO Post (post_id, person_id, person_dat, title, [description], cat, dat)
VALUES (4, 2, '3000-01-01', 'test3', '', @Time1, '3000-01-01')

--initial data
SELECT 'Init data'
SELECT * from Person P 
LEFT JOIN Post Po ON P.person_id = Po.person_id 
LEFT JOIN Photo Ph ON P.person_id = Ph.person_id

--Current time is @Time2
SELECT 'Current time:', @Time2

--soft delete Valya's post with value 'test3'
SELECT 'Soft delete Valya`s post with value `test3`'
UPDATE Post SET dat = @Time2 WHERE title LIKE 'test2'
SELECT * from Post

--soft delete Alex (with marking depended records as deleted)
SELECT 'Soft delete Alex'

DECLARE @Alex_id INT
DECLARE @Alex_dat DATETIME2
SET @Alex_id = 1
SET @Alex_dat = '3000-01-01'

UPDATE Person SET dat = @Time2 WHERE person_id = @Alex_id AND dat = @Alex_dat
UPDATE Post SET dat = @Time2, person_dat = @Time2 WHERE person_id = @Alex_id AND person_dat = @Alex_dat
UPDATE Photo SET dat = @Time2, person_dat = @Time2 WHERE person_id = @Alex_id AND person_dat = @Alex_dat

SELECT * from Person P 
LEFT JOIN Post Po ON P.person_id = Po.person_id 
LEFT JOIN Photo Ph ON P.person_id = Ph.person_id
WHERE P.person_id = @Alex_id

--Current time is @Time3
SELECT 'Current time:', @Time3

--soft update Valya (set name to Valentina)
DECLARE @Valya_id INT
DECLARE @Valya_dat DATETIME2
SET @Valya_id = 2
SET @Valya_dat = '3000-01-01'

--#1 soft update Valya's photo
SELECT 'Soft update Valya`s photo'
INSERT INTO Photo (photo_id, person_id, person_dat, [value], cat, dat)
SELECT Ph.photo_id, P.person_id, @Time3, Ph.[value], Ph.cat, @Time3 FROM Person P, Photo Ph WHERE P.person_id = @Valya_id 
AND P.dat = @Valya_dat 
AND Ph.person_id = P.person_id 
AND Ph.person_dat = P.dat

UPDATE Photo SET cat = @Time3 WHERE person_id = @Valya_id AND person_dat = @Valya_dat

--#2 soft update Valya's posts
SELECT 'Soft update Valya`s posts'
INSERT INTO Post (post_id, person_id, person_dat, title, [description], cat, dat)
SELECT Po.post_id, P.person_id, @Time3, Po.title, Po.[description], Po.cat, @Time3 FROM Person P, Post Po WHERE P.person_id = @Valya_id
AND P.dat = @Valya_dat 
AND Po.person_id = P.person_id 
AND Po.person_dat = P.dat

UPDATE Post SET cat = @Time3 WHERE person_id = @Valya_id AND person_dat = @Valya_dat

SELECT 'Soft delete Valya'
--create copy of current record
INSERT INTO Person(person_id, name, cat, dat) 
SELECT @Valya_id, name, cat, @Time3 FROM Person WHERE person_id = @Valya_id AND dat = @Valya_dat

--modify curent record
UPDATE Person SET name = 'Valentina', cat = @Time3 WHERE person_id = @Valya_id AND dat = @Valya_dat

SELECT 'Valentina`s history'
SELECT * from Person WHERE person_id = @Valya_id

SELECT 'Valentina`s photo history'
SELECT * from Photo WHERE person_id = @Valya_id

SELECT 'Valentina`s posts history'
SELECT * from Post WHERE person_id = @Valya_id

SELECT 'Current active database records'
SELECT * from Person P 
LEFT JOIN Post Po ON P.person_id = Po.person_id
LEFT JOIN Photo Ph ON P.person_id = Ph.person_id
WHERE P.dat = '3000-01-01' AND Po.dat = P.dat AND Ph.dat = P.dat

--select database records that is valid at @Time2
SELECT 'Database Person records that is valid at @Time2'
SELECT * from Person 
WHERE   (dat > @Time2 or dat = '3000-01-01')
AND     cat <= @Time2

SELECT 'Database Post records that is valid at @Time2'
SELECT * from Post
WHERE   (dat > @Time2 or dat = '3000-01-01')
AND     cat <= @Time2

SELECT 'Database Photo records that is valid at @Time2'
SELECT * from Photo
WHERE   (dat > @Time2 or dat = '3000-01-01')
AND     cat <= @Time2
-- */

/*
===============END of Attempt #3===============
*/