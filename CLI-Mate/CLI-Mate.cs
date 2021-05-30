using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;

/*
Instructions.
- pwsh.exe -> this will run a new powershell 6 instance within the current one
- exit out of this one to detach the imported module
- dotnet build -> to build the DLL
- Import-Module .\bin\Debug\<somemadmad>\CLI.dll
- call using get-weatherdata <variable>
- link to calling rest apis -> https://stackoverflow.com/questions/53529061/whats-the-right-way-to-use-httpclient-synchronously/53529122
- Placeholder data -> https://jsonplaceholder.typicode.com/posts
*/
namespace CLI_Mate
{
    [Cmdlet(VerbsCommon.Get,"WeatherData")]
    //[OutputType(string)]
    public class GetWeather: PSCmdlet
    {
        [Parameter(
            Mandatory = true,
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true
        )]
        public string Name {get;set;}
        protected override void BeginProcessing()
        {
            WriteVerbose("Begin!");
        }
        //MAIN METHOD 
        protected override void ProcessRecord()
        {           
            var client = new HttpClient();            
            var task = Task.Run(() => client.GetAsync("https://jsonplaceholder.typicode.com/posts")); 
            task.Wait();
            var response = task.Result;
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            Console.WriteLine(Name);
            //WriteObject(new Person{
            //    Name = Name
            //});
            //WriteVerbose(this.Name);            
            //WriteVerbose("Processing!");
        }
        protected override void EndProcessing()
        {
            WriteVerbose("End!");
        }
    }
    public class Person{
        public string Name {get;set;}
    }
    public class Posts
    {
        public int userId {get;set;}
        public int id {get;set;}
        public string title {get;set;}
        public string body {get;set;}
    }
}