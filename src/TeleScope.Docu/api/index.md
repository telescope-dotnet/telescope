# :books: API Documentation

The entire Framework is divided into 15+ NuGet packages that follow Clean Architecture principles.

* The packages are **vertivally sliced** into abstractions and their implementations.
* The **horizontal divisions** are made for each feature like connectors or cross-cutting utility functions. 
* The colored vertical stacks represent the dependencies between the packages from top to bottom.<br>
* There are no horizontal dependencies, except for the logging assembly.

>[!NOTE]
> Follow the links behind the package shortcuts to navigate to the namespace overviews.

<!-- https://www.tablesgenerator.com/html_tables# -->

<style type="text/css">
.tg  {border-collapse:collapse;border-spacing:0;}
.tg td{border-color:black;border-style:solid;border-width:1px;font-family:Arial, sans-serif;font-size:14px;
  overflow:hidden;padding:10px 5px;word-break:normal;}
.tg th{border-color:black;border-style:solid;border-width:1px;font-family:Arial, sans-serif;font-size:14px;
  font-weight:normal;overflow:hidden;padding:10px 5px;word-break:normal;}
.tg .tg-iynb{background-color:#c0c0c0;color:#343434;font-weight:bold;text-align:center;vertical-align:top}
.tg .tg-j62g{background-color:#c0c0c0;color:#343434;text-align:center;vertical-align:top}
.tg .tg-y2pj{background-color:#ffccc9;color:#343434;text-align:center;vertical-align:top}
.tg .tg-03x9{background-color:#ffce93;color:#343434;text-align:center;vertical-align:top}
.tg .tg-766i{background-color:#fffc9e;color:#343434;text-align:center;vertical-align:top}
.tg .tg-bd1l{background-color:#9aff99;color:#343434;text-align:center;vertical-align:top}
.tg .tg-7b8a{background-color:#96fffb;color:#343434;text-align:center;vertical-align:top}
.tg .tg-yrns{background-color:#67fd9a;color:#343434;text-align:center;vertical-align:top}
.tg .tg-nr0s{background-color:#fe996b;color:#343434;text-align:center;vertical-align:top}
.tg .tg-wtib{background-color:#34ff34;color:#343434;text-align:center;vertical-align:top}
.tg .tg-n7am{background-color:#68cbd0;color:#343434;text-align:center;vertical-align:top}
</style>
<table class="tg">
<thead>
  <tr>
    <th class="tg-y2pj" rowspan="3"><a href="TeleScope.GuardClauses.html">GuardClauses</a><br></th>
    <th class="tg-03x9" rowspan="2"><a href="TeleScope.Logging.Extensions.Serilog.html">Extensions.Serilog</a></th>
    <th class="tg-766i" rowspan="2"><a href="TeleScope.UI.Cli.Options.html">Cli</a></th>
    <th class="tg-bd1l"><a href="TeleScope.Connectors.Mqtt.html">Mqtt</a></th>
    <th class="tg-bd1l"><a href="TeleScope.Connectors.Http.html">Http</a></th>
    <th class="tg-bd1l"><a href="TeleScope.Connectors.Plc.Siemens.html">Plc.Siemens</a></th>
    <th class="tg-bd1l"><a href="TeleScope.Connectors.Smtp.html">Smtp</a></th>
    <th class="tg-7b8a" rowspan="2"><a href="TeleScope.Persistence.Json.html">Json</a></th>
    <th class="tg-7b8a" rowspan="2"><a href="TeleScope.Persistence.Yaml.html">Yaml</a></th>
    <th class="tg-7b8a" rowspan="2"><a href="TeleScope.Persistence.Csv.html">Csv</a></th>
    <th class="tg-7b8a" rowspan="2"><a href="TeleScope.Persistence.Parquet.html">Parquet</a></th>
  </tr>
  <tr>
    <th class="tg-yrns"><a href="TeleScope.Connectors.Mqtt.Abstractions.html">Mqtt.Abstractions</a></th>
    <th class="tg-yrns"><a href="TeleScope.Connectors.Http.Abstractions.html">Http.Abstractions</a></th>
    <th class="tg-yrns"><a href="TeleScope.Connectors.Plc.Abstractions.html">Plc.Abstractions</a></th>
    <th class="tg-yrns"><a href="TeleScope.Connectors.Smtp.Abstractions.html">Smtp.Abstractions</a></th>
  </tr>
  <tr>
    <th class="tg-nr0s"><a href="TeleScope.Logging.html">Logging</a></th>
    <th class="tg-j62g">UI</th>
    <th class="tg-wtib" colspan="4"><a href="TeleScope.Connectors.Abstractions.html">Connectors.Abstractions</a></th>
    <th class="tg-n7am" colspan="4"><a href="TeleScope.Persistence.Abstractions.html">Persistence.Abstractions</a></th>
  </tr>
</thead>
<tbody>
  <tr>
    <td class="tg-iynb" colspan="11">TeleScope</td>
  </tr>
</tbody>
</table>


---
Have a nice day! :beers:
