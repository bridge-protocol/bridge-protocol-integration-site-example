<p align="center">
  <img
    src="https://storage.googleapis.com/bridge-assets/Bridge_Logo_Black.png"
    width="125px;">
</p>
<h3 align="center">Bridge Protocol Site Integration Example for .NET</h3>

# Summary
The Bridge Protocol Site Integration Example demonstrates how to accept a Bridge Passport for Authentication and Authorization in an ASP.NET MVC Website.

# Dependencies
- Bridge Protocol Integration Wrapper for .NET (https://github.com/bridge-protocol/bridge-protocol-integrations-dotnet)

# Integration with the Bridge Passport Extension
For integration with the Bridge Passport for Authentication and Authorization, the following block must exist on the login page to facilitate the communication of the login request and response payloads between the site and the extension:

```
<div id="bridge_passport_login">
     <input id="bridge_protocol_passport_login_request" type="hidden" />
     <input id="bridge_protocol_passport_login_response" type="hidden" />
     <input id="bridge_protocol_passport_login_passport_id" type="hidden" />
</div>
```
Optionally, if the parent form is given id="bridge_passport_login_form", it will auto-submit when the passport response is provided.
