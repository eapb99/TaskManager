# Proyecto de Gestión de Tareas

Este repositorio contiene una solución completa para la gestión de tareas, compuesta por tres proyectos:

1. **API**: Un proyecto de API que expone endpoints para gestionar las tareas pendientes.
2. **MVC**: Una aplicación MVC que consume los servicios de la API y proporciona una interfaz de usuario para la gestión de tareas.
3. **Tests**: Un proyecto de pruebas unitarias que verifica la funcionalidad tanto de la API como de la aplicación MVC.

## Estructura de la Solución

La solución está organizada en tres proyectos separados dentro de una única solución:

- `TaskManagerAPI`: Contiene la API que maneja la lógica de negocio y los datos.
- `TaskManagerMVC`: La aplicación MVC que interactúa con los usuarios finales.
- `TaskManagerTests`: El proyecto de pruebas que contiene las pruebas unitarias para ambos, la API y el MVC.

---

## 1. API (`TaskManagerAPI`)

### Descripción

El proyecto de la API está construido utilizando ASP.NET Core y proporciona una serie de endpoints para gestionar las tareas pendientes. Utiliza Entity Framework Core para la interacción con la base de datos.

### Endpoints Principales

- **GET /api/PendingTask**: Devuelve todas las tareas pendientes.
- **GET /api/PendingTask/{id}**: Devuelve una tarea específica por su ID.
- **POST /api/PendingTask**: Crea una nueva tarea.
- **PUT /api/PendingTask/{id}**: Actualiza una tarea existente.
- **DELETE /api/PendingTask/{id}**: Elimina una tarea por su ID.

### Autenticación y Autorización

- Los endpoints están protegidos con `[Authorize]`, lo que requiere que los usuarios estén autenticados para interactuar con la API.
- La autentica se implemento mediante Identity.
- La autenticación se realiza mediante JWT (JSON Web Tokens).

### Estructura del Proyecto

- **Controllers**: Contiene el controlador `PendingTaskController` que maneja todas las operaciones CRUD.
- **Models**: Define la clase `PendingTask` que representa una tarea pendiente.
- **Data**: Incluye `TaskDbContext`, que maneja la interacción con la base de datos utilizando Entity Framework Core.

### Ejecución del Proyecto

Para ejecutar la API, utiliza el siguiente comando en la consola:

```bash
dotnet run --project TaskManagerAPI
```
## 2. MVC (`TaskManagerMVC`)

### Descripción

El proyecto MVC actúa como la interfaz de usuario de la solución. Esta aplicación consume los endpoints de la API y proporciona una manera intuitiva para que los usuarios gestionen sus tareas.

### Funcionalidades 

- **Inicio de Sesión**:Los usuarios deben iniciar sesión para acceder a la aplicación.
- **Gestión de Tareas**: Permite a los usuarios crear, ver, editar y eliminar tareas.
- **Interfaz de Usuario Amigable**: Desarrollada utilizando Razor Views que consumen los servicios de la API.


### Estructura del Proyecto

- **Controllers**: Contiene el controlador `TasksController` que gestionan las vistas y la lógica del flujo de trabajo del usuario.
- **Views**:Carpeta que contiene las vistas Razor para cada funcionalidad de la aplicación.
- **Services**: Incluye ``ApiService``, que se encargan de comunicarse con la API para obtener y enviar datos..

### Ejecución del Proyecto

Para ejecutar la aplicación MVC, usa el siguiente comando:
```bash
dotnet run --project TaskManagerMVC
```

##  3. Proyecto de Pruebas (`TaskManagerTests`)

### Descripción

El proyecto de pruebas contiene todas las pruebas unitarias necesarias para verificar la funcionalidad de los endpoints de la API, el servicio de comunicacion y  la aplicación MVC. Utiliza xUnit como framework de pruebas y Moq para simular dependencias.

### Estructura del Proyecto

- **Controllers Tests**: Contiene pruebas unitarias para el `PendingTaskController,TaskController`.
- **Service Tests**: Pruebas para los servicios utilizados dentro de la aplicación MVC  `ApiService`.
- **Setup del Contexto de Pruebas**: Se utiliza una base de datos en memoria (`InMemoryDatabase`) para simular las operaciones de la base de datos durante las pruebas.

### Principales Pruebas
- **ApiServiceTests**
	- **GetTasksAsync_ReturnsTasks**: Verifica que el servicio devuelva todas las tareas correctamente.
- **PendingTaskControllerTests**
	- **GetPendingTask_ReturnsTask_WhenIdIsValid**: Verifica que el api devuelva la tarea basado en un id.
- **TasksControllerTests**
	- **Index_ReturnsViewResult_WithListOfTasks**: Verifica que la vista Index devuelva todas las tareas de manera correcta.

### Ejecución de las Pruebas

Para ejecutar las pruebas, utiliza el siguiente comando:

```bash
dotnet test
 ```

## Configuración de Desarrollo

### Requisitos Previos

- **.NET Core SDK**: Necesario para compilar y ejecutar los proyectos.
- **Visual Studio o Visual Studio Code**: Recomendado para desarrollar, depurar y ejecutar el proyecto.
- **SQL Server**: Requerido para la ejecución local de la API si se utiliza una base de datos real en lugar de la base de datos en memoria para pruebas.

### Configuración Inicial

1. **Clonar el repositorio.**

   ```bash
   git clone <url-del-repositorio>
   ```
2. **Restaurar los paquetes NuGet ejecutando**

   ```bash
     `dotnet restore`
   ```
3. **Configurar las cadenas de conexión para la base de datos en `appsettings.json` dentro del proyecto de la API**