# [10FastFingers.com Hacking](https://10fastfingers.com/typing-test/portuguese)  in C# #

In this repository you'll find a bot that makes the computer write for you.
The bot will access the 10FastFingers.com e starts to type every word in the textbox, with an average of 413WPM
Next, the bot will capture the result of the typing and save in a table on PostgreSQL Database.

![example_selenium](https://github.com/pasklan/SeleniumBot/assets/26265067/5c8e87d5-69b9-44a5-ad02-cc66642845cd)

![screenshot](https://github.com/pasklan/SeleniumBot/assets/26265067/43ee4a15-a0d3-469e-9c8a-ede780475220)

# Requirements #

[Visual Studio 2022](https://visualstudio.microsoft.com/pt-br/)

## Packages: ##

### [Selenium](https://www.selenium.dev/pt-br/documentation/webdriver/getting_started/install_library/) ###
- Selenium v4.18.1 WebDriver (NuGet name: Selenium.WebDriver)
- Selenium Support v4.18.1 (NuGet name: Selenium.Support)
- Selenium Chrome Driver v122.0.626 (NuGet name: Selenium.WebDriver.ChromeDriver)


### [PostGreSQL](https://www.postgresql.org/download/windows/) ###
- PostgreeSQL v16
- pgAdmin v8.2
- .NET data provider for PostgreSQL v8.0.2 (NuGet name: [Npgsql](https://github.com/npgsql/npgsql))

  
## Database Config ##

For the bot to work, you need to create and configure a PostGreSQL database:

###  Database name: ***typing_test_db*** ### 
Create Comand:
```
CREATE DATABASE typing_test_db
    WITH
    OWNER = postgres
    ENCODING = 'UTF8'
    LOCALE_PROVIDER = 'libc'
    CONNECTION LIMIT = -1
    IS_TEMPLATE = False;
```

### Table name: ***WORD_RESULT*** ###
```
CREATE TABLE public."WORD_RESULT"
(
    "ID" serial NOT NULL,
    "WPM" bigint NOT NULL,
    "KEYSTROKES" bigint NOT NULL,
    "ACCURACY" text NOT NULL,
    "CORRECT_WORD" bigint,
    "WRONG_WORD" bigint,
    PRIMARY KEY ("ID")
);

ALTER TABLE IF EXISTS public."WORD_RESULT"
    OWNER to postgres;
```

# How it works #

The bot consists of 4 classes:
## PostgreeCon ##
Defines the PostgreSQL database connection, it uses the Interface Disposable to free the connection automatically after the execution
- The **PostgreCon()** method create and open the connection
- The **Dispose()** methos free the connection

## Query ##
Defines 2 methods to manipulate the table *WORD_RESULT*
- **InsertData()** method insert the collected result from the web scrapping into the table
- **SelectDataAll()** method select all the lines in the *WORD_RESULT*

## Automation ##
Where the typing and capture data are made, the classes handle with the WebDriver and prepare the result to be inserted into the table *WORD_RESULT*
- Constructor **Automation()** initialize the Chrome WebDriver in a Maximized window
- **CloseWebDrive()** closes the Browser
- **GoToWebSite()** navigate to the URL informed, the method close the Ads and cookies pop-up that get in the way of the textbox
- **HandleAlert()** will close the alert pop-up that shows before the result appears
- **IsAlertPresent()** verify if the alert pop-up exists
- **PopulateText()** capture every word displayed in the text box and type in the input box, after the end of the typing, it's necessary wait until the timer ends,
the default value used is 45 seconds, after the wait time, the result is captured.
- **SanitizeWords()** is the last method and cleans every needed data, most of the processing is done with Regular Expressions

## Program ##
Is where the **Main()** function is, it controle the steps taken by the bot:
  1. Instance the Automation class where the typing will be made
  2. Go to the URL informed 
  3. Start the typing process
  4. Instance the PostgreSQL connection  
  5. Instance the query that will be used
  6. Saves the result data from the webscraping into the database
  7. Retrieves the data from the table WORD_RESULT to consult and display in the Terminal Console
