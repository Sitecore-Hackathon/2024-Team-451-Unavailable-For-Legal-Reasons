# Contributing

...

## GraphQL

Headless endpoint UI:

- <https://cm.team451.localhost/sitecore/api/graph/edge/ui>
- Use `{ "sc_apikey": "{F0107448-59B2-40D0-9158-B6F33F17E9C4}" }`

Authoring endpoint UI:

- <https://cm.team451.localhost/sitecore/api/authoring/graphql/playground/> (ensure server url is `https://cm.team451.localhost/sitecore/api/authoring/graphql/v1/`)
- Get bearer token with: `curl.exe -k --data "client_id=newsmixer&grant_type=password&username=sitecore\admin&password=b" -H "Content-Type: application/x-www-form-urlencoded" "https://id.team451.localhost/connect/token"`
- Use `{ "Authorization": "Bearer XXX" }`
