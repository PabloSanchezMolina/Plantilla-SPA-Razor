# Plantilla-SPA-Razor
Plantilla de ejemplo de implementación de aplicación web SPA en .NET con Razor


Esta plantilla no tiene acceso a datos, solo conexión a servidor local de caché (Redis). Tampoco hay ninguna autenticación, se hardcodea un único usuario.

Se trata de un ejemplo de implementación de una aplicación web SPA que guarda el estado actual del usuario en Redis y utiliza el mismo motor Razor para renderizar la información del modelo utilizando la plantilla con sintaxis Razor, sin necesidad de hacer posteriormente data-binding con ningún framework javascript en cliente.

La arquitectura de este enfoque está inspirada en el patrón Flux. Todo el sitio web es una máquina con estados, solo que muy simplificada, facilitando así la implementación y el mantenimiento del código en el equipo de desarrollo.

La vista Razor de la primera carga (primera y única request, pues se trata de un enfoque SPA) aplica el modelo almacenado en Redis (o inicializado, en el caso de nuevos usuarios) según el estado actual de la aplicación para nuestro usuario. Cada vez que éste interactúa, cada enlace que redirige, los inputs que activan eventos onchange, etc. Todos ellos lanzarán desde javascript una solicitud post a backend pasando la información necesaria que backend procesará. Modificará el estado si procede y lo persistirá en Redis.

Después, generará el html resultante de aplicar el modelo a la vista Razor que corresponde al estado actual de la aplicación. Ese html se devuelve en la response a javascript, cuya única finalidad es pisar el contenido anterior con el html nuevo (reducir a una sola sentencia la actualización del dom de la página es más eficiente desde el punto de vista del navegador). De esta manera evitamos implementar mediante javascript el data-binding que modifique la vista a partir del modelo, planteamiento de la mayoría de frameworks SPA de cliente.

La solución la he organizado de esta manera:
	1) Api:
		*) El controlador con los Post para las llamadas Ajax
		*) La clase que genera el html al aplicar un modelo a una vista razor
		*) Las clases para definir el objeto devuelto y los parámetros recibidos
	2) DAO: 
		*) La gestión de la actividad del usuario (dividido en una partial class para cada estado)
		*) La gestión de la caché (Redis)
	3) DTO:
		*) Una clase con el modelo por cada vista Razor que utilizaremos en el proyecto.
	4) Pages:
		*) Dividido en carpetas: una por cada estado.

Nota: En los partial names hay que utilizar rutas absolutas, porque las rutas relativas serán distintas dependiendo si se aplican desde la vista Razor que la incluya (en la primera carga) o si se leen al renderizar el modelo contra la vista desde la clase de Renderizado.
