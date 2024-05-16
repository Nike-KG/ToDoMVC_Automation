# ToDoMVC_Automation
## Tools and Technologies used - 
* Selenium Web Driver
* C#
* NUnit Test framework

### Desing pattern
* Page Object Model design pattern

#### Why Page Obeject Model? 
POM(Page Object Model) design patteren is used since this design pattern avoid repetition, enhanced the maintainability and, promotes modularity by separating the web pages or UI components into separate classes. POM allows for reusability of code. Once a page or component class is defined, it can be reused across multiple test cases. Also, POM enhances the readability of test scripts. Test cases become more self-explanatory and easier to understand.

### Reporting
* ExtentReports

#### Why ExtentReports? 
ExtentReports is used since it generates rich and interactive HTML reports that include detailed information about test executions, such as test case status (pass/fail/skip), logs, and screenshots. It also provides a dashboard view that gives a high-level summary of test execution results, including the overall pass/fail status, execution duration, and charts.

## Requirements
* .net SDK 8.0
* Visual Studio or Visual Studio Code.

## Build
#### Option 01 - Build by Command
Navigate to project folder and check *.csproj file exists in the folder. 

``` 
dotnet restore 
dotnet build
```

#### Option 02 - Build using Visual Studio
1. Open .sln file in project folder.
2. Go to Build --> Rebuild Solution.

## Run Test
#### Option 01 - Run by Command
Navigate to project folder and check *.csproj file exists in the folder. 

```  
dotnet test

```
### Option 02 - Run using Visual Studio
1. Open .sln file in project folder.
2. Go to Test --> Run All Tests.

## Test Results Report and Log

### Report - Folder path (ToDoMVC/TestResults/extent-report.html)
#### Option 01 - Detailed Report - Graph View
![image](https://github.com/Nike-KG/ToDoMVC_Automation/assets/134206700/0b7840b1-19de-409e-b9bb-68c9f517fc9f)

#### Option 02 - Detailed Report - Detailed View
![image](https://github.com/Nike-KG/ToDoMVC_Automation/assets/134206700/3c46d9a3-ca8f-40cf-993f-b308e14c92de)

### Log
![image](https://github.com/Nike-KG/ToDoMVC_Automation/assets/134206700/be36cf96-e5f9-4d34-a778-147cf7042ba3)



### Test Scenarios (ToDoMVC/Testscenarios/Test Scenarios_ToDoMVC.xlsx)
Test Scenarios document can be found in "Test Scenarios" folder in the project.
![image](https://github.com/Nike-KG/ToDoMVC_Automation/assets/134206700/2dfcb687-8815-45fc-b244-0fa5c7e641b9)
