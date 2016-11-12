This solution demonstrates operations and load tests between ASP.NET Core Web Client and ASP.NET Core Web API.
WebClient consumes backend APIs over Backend.Hosting with 5010 port and Backend.Hosting responsible for long running process and respond
to the WebClient.

Its working when user interact with browser page submit simultaneously very well!
When increase the number of concurrent request to 50 within total 200 request with command line tool [Hey](https://github.com/rakyll/hey) then
both are hanging and not respond.

###Test Environment:
IIS 10,
Windows 10 Pro Version 1607
i7 2.40GHz 4 Cores, 240 GB SSD Hard Disc, 32GB RAM 

![Image](https://github.com/gencebay/NetCoreSmallLoadRepro/blob/master/ReproNetCore.gif)