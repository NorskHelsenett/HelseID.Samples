# NHN Persontjenesten - Getting started

## _0. Swap out HelseId credentials!_
_Please you your own [HelseId](https://selvbetjening.nhn.no/) credentials!_

The ones provided in this repository is just for example.
There is no guarantee that they will work.

---

## 1. Install needed tool
Install [NSwag](https://github.com/RicoSuter/NSwag) to generate the CSharp client based on the Open API definition.

```
dotent tool install -g nswag.consolecore
```

## 2. Generate the first definition

```
nswag run persontjenesten.nswag
```

## 3. Build the project
To trigger the NSwag MSBuild task we need to build the project in `Debug`

```
dotnet build Persontjenesten.csproj -c Debug
```

## 4. Do a test run

```
dotnet run
```