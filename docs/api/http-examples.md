### Base URL
@baseUrl = http://localhost:5080

### Register (sem indicação)
POST {{baseUrl}}/auth/register
Content-Type: application/json

{
  "name": "Ana Silva",
  "email": "ana{{Math.floor(Math.random()*9999)}}@mail.com",
  "password": "Senha1234"
}

### Register (com indicação ?ref=XXXX)
POST {{baseUrl}}/auth/register?ref=ABCDEFGH
Content-Type: application/json

{
  "name": "Bruno Souza",
  "email": "bruno{{Math.floor(Math.random()*9999)}}@mail.com",
  "password": "Senha1234"
}

### Login
POST {{baseUrl}}/auth/login
Content-Type: application/json

{
  "email": "ana@mail.com",
  "password": "Senha1234"
}

### Me (autenticado)
@jwt = {{login.response.body.token}}
GET {{baseUrl}}/me
Authorization: Bearer {{jwt}}