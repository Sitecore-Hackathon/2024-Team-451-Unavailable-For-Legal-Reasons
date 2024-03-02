if ($null -eq $data)
{
    $data = Get-Content -Path $PSScriptRoot\sample-data.json | ConvertFrom-Json
}

$query = @"
mutation {
  createItem(
    input: {
      name: "%NAME%"
      templateId: "{FF095022-530E-46AC-BC22-816A11C3A1BD}"
      parent: "{110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9}"
      language: "en"
      fields: [
        { name: "Title", value: "%TITLE%" }
        { name: "Body", value: "%BODY%" }
      ]
    }
  ) {
    item {
      itemId
    }
  }
}
"@

$token = (curl.exe -k -s --data "client_id=newsmixer&grant_type=password&username=sitecore\admin&password=b" -H "Content-Type: application/x-www-form-urlencoded" "https://id.team451.localhost/connect/token") | ConvertFrom-Json

$counter = 0
$data.articles | ForEach-Object {
    $rawData = @{
        "operationName" = $null;
        "variables"     = $null;
        "query"         = $query.Replace("%NAME%", "article$counter").Replace("%TITLE%", $_.title).Replace("%BODY%", $_.content)
    }
    | ConvertTo-Json

    curl.exe -k -v -H "Authorization: Bearer $($token.access_token)" -H "content-type: application/json" --data-raw $rawData "https://cm.team451.localhost/sitecore/api/authoring/graphql/v1/"

    $counter++
}
