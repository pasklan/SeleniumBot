using OpenQA.Selenium.DevTools.V120.WebAudio;
using SeleniumBot;


class Program
{
    static void Main(string[] args)
    {
        // Steps taken by the automation:
        //1. Instance the Automation class where the typing will be made
        //2. Go to the URL informed 
        //3. Start the typing process
        //4. Instance the PostgreSQL connection  
        //5. Instance the query that will be used
        //6. Saves the result data from the webscraping into the database
        //7. Retrieves the data from the table WORD_RESULT to consult

        var web = new Automation();
        var url = "https://10fastfingers.com/typing-test/portuguese";
        web.GoToWebSite(url);
        var webResult = web.PopulateText();
        web.CloseWebDrive();

        var db = new PostgreCon();
        var query = new Query();
        Query.InsertData(db.Connection, webResult);
        List<string[]> response = Query.SelectDataAll(db.Connection);
        Console.WriteLine("Dados do banco: ");
        foreach (var responseItem in response)
        {
            Console.WriteLine("WPM: " + responseItem[1] + " KEYSTROKE: " + responseItem[2] + " ACCURACY: " + responseItem[3] + " CORRECT WORDS: " + responseItem[4] + " WRONG WORDS: " + responseItem[5] );
        }
    }
}