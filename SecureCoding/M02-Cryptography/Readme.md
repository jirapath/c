### Cryptography
1. HMAC hashing
```
const crypto = require("crypto");
const key = crypto.randomBytes(256).toString('hex');
const hmac = crypto.createHmac('sha256', key);
const data = 'something you want to hash';
hmac.update(data);
console.log(hmac.digest('hex'));
```

1. PBKDF2 hashing
```
const crypto = require("crypto");
const password = "password1";
const salt = crypto.randomBytes(256).toString('hex');
const hashedPassword = crypto.pbkdf2Sync(password, salt, 100000, 512, 'sha512');
console.log(hashedPassword.toString('hex'));
```

1. AES Encryption
```
const crypto = require("crypto");
const algorithm = 'aes-256-gcm';
const credential = 'secret';

const salt = crypto.randomBytes(32);
const keyCryption = crypto.scryptSync(credential, salt, 32);
const iv = crypto.randomBytes(16);

// Encrypt data
const cipher = crypto.createCipheriv(algorithm, keyCryption, iv);
let secretData = 'This is a secret text';

// [Ex. change normal text to ascii]
let encrypted = cipher.update(secretData, 'utf8', 'hex');
encrypted += cipher.final('hex');
console.log(encrypted);

const tag = cipher.getAuthTag();
console.log(tag);

// Decrypt data
const decipher = crypto.createDecipheriv(algorithm, keyCryption, iv);
decipher.setAuthTag(tag);
// [Ex. change ascii string to normal text]
let decrypted = decipher.update(encrypted, 'hex', 'utf-8');
decrypted += decipher.final('utf-8');
console.log(decrypted);
```


1. Diffie Hellman (Key Exchange)
```
const crypto = require("crypto");
const sally  = crypto.createDiffieHellman(2048);
const sallyKey = sally.generateKeys();
sallyKey.toString('hex');


const bob = crypto.createDiffieHellman(sally.getPrime(), sally.getGenerator());
const bobKey = bob.generateKeys();
bobKey.toString('hex');

const bobSecret = sally.computeSecret(bobKey);
const sallySecret = bob.computeSecret(sallyKey);

console.log(bobSecret.toString('hex'));
console.log(sallySecret.toString('hex'));
```