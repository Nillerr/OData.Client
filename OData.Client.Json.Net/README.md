# OData.Client.Json.Net

```
nuget add OData.Client.Json.Net
```

## Usage

```c#
using OData.Client.Json.Net;

var oDataClientSettings = new ODataClientSettings(
    organizationUri: "[Organization URI]",
    propertiesFactory: new JsonNetPropertiesFactory(),
    entitySerializerFactory: new JsonNetEntitySerializerFactory()
);

var oDataClient = new ODataClient(oDataClientSettings);
```