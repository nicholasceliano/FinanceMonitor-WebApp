FinanceMonitor Web App
===============================
**Objective:** Create an application where I can consolidate the financial status from various monetary accounts(401k, savings, checking, credit card) and track my total net worth overtime.

**Application Description:** This application has three tiers. The SQL database was relational and included tables, stored procedures, and triggers. The middle-ware of this application was written in C# and contained the server side logic and the Web API web services which were exposed for the UI. The middle-ware tied into the necessary bank accounts by either using HTTP web requests to log in to the institution's website and scrape data, or by using the third party 'Plaid API' for applicable banks. To collect the data there was an Azure daily job which ran at 6 AM daily and made a web service call to the application to do a refresh account totals. Also integrated into the middle-ware was the use of a third party Azure email service which sent out daily and/or weekly Profit/Loss emails on the users accounts. The iOS app(see https://github.com/nicholasceliano/FinanceMonitor-iOSApp) was developed as the UI for the data. The users have the ability to register for a new login and add various bank accounts for tracking capabilities. The SQL database and C# .NET application both live on Azure.

**Conclusion:** I ended up using this application for a few months. It worked well and I was getting all the consolidated information I was looking for. I found out that the iOS app was unnecessary(for my use) and I was able to get all the information I was looking for from the daily/weekly emails. After a few weeks of use I received fraud alerts from one of the banks I was connecting to via bouncing HTTP requests. Since I was connecting to 3/5 accounts through this process I ended up deciding to shut down the service to avoid the headaches. I figured I would wait for Plaid API to open up API's to more banks but they are still a work in progress.

**Technologies Used:** MVC, Web API, C#, SQL, iOS development, Objective C, Azure Hosting, SendGrid Email, Plaid API, Job scheduling

---

**Web API Tests**

<img src="https://raw.githubusercontent.com/nicholasceliano/FinanceMonitor-WebApp/master/Images/FinanceMonitorWebApp.png" />
