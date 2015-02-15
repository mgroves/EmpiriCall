# EmpiriCall

Record and display information about how your ASP.NET MVC site is being used.

# Quick Start

There are only 4 easy steps to get EmpiriCall running right now.

1. Add a reference to EmpiriCall. You'll also want an EmpiriCall data provider like EmpiriCall.Data.SQLServer (technically not required, you could write your own). Once I get EmpiriCall on NuGet, then you've probably already done this.

2. To view the console and reports for EmpiriCall, you must add an HttpHandler to your Web.config. It's as simple as:
```
<system.webServer>
	<handlers>
		<add name="EmpiriCallReports" verb="*" type="EmpiriCall.EmpiriCallReportHandler, EmpiriCall" path="EmpiriCall.axd" resourceType="Unspecified" />
	</handlers>
</system.webServer>
```

3. EmpiriCall is an MVC ActionFilter. Somewhere in your application start (perhaps Global.ascx's Application_Start'), add it as a global filter:
```
GlobalFilters.Filters.Add(new EmpiriCallActionFilter());
```

4. Tell EmpiriCall which data provider you're using. This can probably go in Global.ascx too (perhaps Application_BeginRequest). I only have one provider so far, here's how to use it:
```
// SQL Server (with Entity Framework)
EmpiriCallConfig.LoadDbContainer(new SqlServerResolver([a DbConnection));
```

# How does the console work?

It's super ugly right now, but it is functional.

## Raw Detail

This is the raw output of each action that has been called: its controller, action, and parameter type/names, when it was called,

## Force Meta Update

This is probably the first thing you should do. It will scan your assemblies to find ALL the controllers, actions, and parameter type/names.

## Show Meta Data

This will show all the meta data that was collected by the Force Meta Update scan.

## Show Call Data

This will show all the meta data, and how many times each action has been called. 0 if it's not been called yet.

## Load Feature Maps Into Meta Data

If you have created a feature mapper and loaded it into EmpiriCall, then this command will actually run that loader.

# Feature Maps

Controller name, action name, parameter name/types don't typically mean much to a business analyst. What you can do to make this data more meaningful is to actually map those actions to a feature.

You can do this by implementing the ```IFeatureMapper``` interface. This has one method: ```void Map(MapFeature)```. Use the MapFeature argument as a kind of fluent API to map specific controller actions to a feature name (which is just a string).

For instance:

```
public class AopBlogFeatureMapper : IFeatureMapper
{
    public void Map(MapFeature map)
    {
        map
            .Of<AccountController>(c => c.Index(null), "Account Management")
            .Of<AccountController>(c => c.Meta(null), "Account Management")
            .Of<AccountController>(c => c.Post(null), "Account Management");

        map
            .Of<TaxFormController>(c => c.Index(), "Tax Calculation")
            .Of<TaxFormController>(c => c.Index(0, null), "Tax Calculation");

        map
            .Of<ContactController>(c => c.Index(), "Correspondence")
            .Of<ContactController>(c => c.PostContact(null), "Correspondence");
	}
}
```

The above code is basically saying: "The index, meta, and post actions in AccountController are all used for Account Management". "The index actions on TaxFormController are all part of the Tax Calculation feature". And "the index and postcontact methods are all used for the Correspondence feature".

You can make the feature strings be as granular as you need.

Note that you have to specify arguments in those lambdas, but those argument values are not used. Just use whatever placeholder value suits you.

# Configuration

TODO

## User name

TODO

## Custom values

TODO

# Why?

There is a gap between business analytics and code analytics. Business analytics are focused on what the user is doing, classifying the user, helping the user find relevant content, etc. Code analytics are focused on the developer, helping the developer find code that needs refactoring, that isn't performant enough, etc.

These are both great. However, I think there also needs to be some analytics that are valuable to both the business people and the technical people (e.g. between Analyst/Sales & the Product Team).

I created EmpiriCall to answer questions like:

* Which features are never being used?
* Which features are used the least?
* What features does a customer use the most/least?
* How many different users use a feature?

If we know the answers to these questions, then we can use it to make decisions like:

* Can we drop a feature?
* How can we price/package features?
* Where can we focus our coding efforts to have the most impact on actual customers?
* How do we prioritize our backlog?

I know that EmpiriCall can't be the first tool to try and answer these questions and inform these decision, but I believe it approaches the problem in a way that combines the technical knowledge of the developers and the business knowledge of the sales/analyst.

The result is: **empirical evidence**.

# I don't want to use SQL+Entity Framework like in your provider

You can write your own data provider that uses MySQL, SQL with some other ORM, Raven, whatever you want.

Create an implementation of IDependencyResolver that will return the various DB command/query objects (see SqlServerResolver for examples).

Create implementations of the various queries/commands and handlers. There are only 5 at the time of this writing:
```
CommandMetaDataUpdate
CommandAddRecord
QueryRawDetail
QueryGetMetaData
CommandMapFeature
```

# License

EmpiriCall, EmpiriCall.Data, EmpiriCall.Data.SQLServer are all licensed under the MIT License: http://choosealicense.com/licenses/mit/

# TODO

* Meta data management / versioning
* Finish this README
* Move stuff into a wiki
* Ability to add/edit feature mapping in the UI.
* Better looking console/reports.
* More reports for username, custom values, date/time.