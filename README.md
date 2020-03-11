Examen Mercadolibre.

Propietario: Jonatan Morales

Url Api Rest: http://apirestml-dev.us-west-2.elasticbeanstalk.com/api/mutante/mutant
Metodo: POST
Herramienta sugerida: Postman.
Lenguaje Utilizado: C# .Net

Respuestas de la API:
	- Status 200 OK. El ADN es mutante.
	- Status 403 Forbidden. El ADN no es mutante.
	- status 400 Bad Request. El ADN ingresado no cumple con los requisitos de validacion.

Modelo de ADN.

Es mutante. Status 200 OK

[
 "ATGC",
 "CACT",
 "CCAT",
 "CGAA"
]

No es mutante. Status 403

[
 "ATGC",
 "CAGT",
 "CAGT",
 "AGAA"
]

ADN incorrecto. Status 400

[
 "ATC",
 "CAT",
 "CCAT",
 "CGAA"
]

Validaciones.
	- Letras permitidas son A-T-C-G.
	- Cantidad minima de columnas y filas 4.
	- La longitud de cada elemento del ADN debe ser igual a la cantidad de elementos del ADN.
