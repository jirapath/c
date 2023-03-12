# Getting start with HashiCrop Vault
### Start Vault server in dev mode
```
vault server -dev
$env:VAULT_ADDR="http://127.0.0.1:8200" => Add environment variable for Vault.
vault login
```

### Authentication command
```
vault auth list
vault auth enable userpass
vault auth disable userpass => Use for disable user
vault path-help auth/userpass => Use to find path
vault write auth/userpass/users/demo password=demo
vault login --method=userpass username=demo
```

### Working with secret engine
```
vault secrets list
vault secrets enable -path=mysecret kv
vault secrets enable -path=secretv2 kv-v2
vault secrets tune -description="describe your path here" mysecret
vault secrets move mysecret mysecret-v1
vault secrets disable mysecret-v1
[LAB 2022-01-11] ---------------------------------------
vault secrets enable -path=demo/mysecret kv
--------------------------------------------------------
```

### [LAB 2022-01-11] ---------------------------------------
```
// Example Secrets
demo/mysecret/    => Use kv-v1
secret (default)  => Use kv-v2

// KV V1
vault kv put demo/mysecret/apikey token=0123456789
vault kv list demo/mysecret
vault kv get demo/mysecret/apikey

// KV V2
vault kv put secret/apikey token=0123456789
vault kv get secret/apikey
vault kv put secret/apikey token=6789012345 => Update data for next version (0 - X)
vault kv get secret/apikey
vault kv put secret/apikey token=4567890123 => Update data for next version (0 - X)
vault kv get -version=1 secret/apikey
vault kv delete secret/apikey => Delete latest version
vault kv delete -version=1 secret/apikey => Delete with expected version
vault kv undelete -versions=3 secret/apikey => Un delete (Must have version)
vault kv get secret/apikey

// Rollback token
vault kv rollback -version=1 secret/apikey

// Difference bettween put and patch
put     => replace
patch   => update/edit
PATTERN => vault kv patch <KEY_PATH> <TOKEN_NAME_1>=<STRING> ... <TOKEN_NAME_N>=<STRING>
vault kv patch secret/apikey token2=asdggwer token3=asdfghjkwert

//Create a new token
vault token create

// Use token limited using time
vault token create -ttl=5m

// Increase time for token
vault token renew -increment=5m hvs.gMKfMB7jyqaqN0gxoXNnV9Za

// Inspect token all detail
vault token lookup hvs.gMKfMB7jyqaqN0gxoXNnV9Za

// Inspect token by hide secret detail
vault token lookup -accessor knykVTlRFCfzkO8Zo9QYbL8O

// Check usage role of this token
vault token capabilities hvs.gMKfMB7jyqaqN0gxoXNnV9Za

// Revoke the token
By accessor         =>  vault token revoke -accessor knykVTlRFCfzkO8Zo9QYbL8O
By default          =>  vault token revoke hvs.gMKfMB7jyqaqN0gxoXNnV9Za

// Policies
List policies       =>  vault policy list
Write policy
Example write policy ...............................
path "demo/mysecret/*"{
capabilities = ["read"]
}
path "secret/*"{
capabilities = ["read"]
}
....................................................
Method
Use the *.hcl file to create the policy
Command:
Go to policy file directory     =>  cd /Users/<USER_NAME>/<PATH_TO_FILE>/securecode/M03-Vault/
Use write policy               =>  vault policy write dev-policy dev-policy.hcl
Read policy                    =>  vault policy read dev-policy
--------------------------------------------------------
```

### Writing secret value
```
vault kv put secretv2/apikeys/demo token=0123456789
vault kv list secretv2/apikeys
vault kv get secretv2/apikeys/demo
vault kv get -version=1 secretv2/apikeys/demo
vault kv delete -version=1 secretv2/apikeys/demo
vault kv destroy -version=1 secretv2/apikeys/demo
vault secret enable mysecret kv
vault kv enable-versioning mysecret
```

### Working with token
```
vault token list
vault token create
vault token create -ttl=5m
vault token renew -increment=5m
vault token lookup -accessor [accessor]
vault token capabilities [token] mysecret/apikeys
vault token revoke [token]
vault token revoke -accessor [accessor]
```

### Working with policy
```
vault policy list
vault policy write dev-policy dev-policy.hcl
vault policy read dev-policy
vault policy delete dev-policy
vault token create -policy=dev-policy
vault write auth/userpass/users/demo token_policy="dev-policy"
```
