# 🎵 Proyecto AA1 - Aplicación de Música

Una aplicación de consola desarrollada en C# .NET 8.0 que simula una plataforma de música digital con funcionalidades completas de gestión de usuarios, artistas, álbumes, canciones y listas de reproducción.

## 📋 Características

### 🎧 Funcionalidades Principales
- **Gestión de Usuarios**: Registro, inicio de sesión y perfiles de usuario
- **Catálogo Musical**: Gestión de artistas, álbumes y canciones
- **Listas de Reproducción**: Creación y administración de playlists personalizadas
- **Reproductor de Audio**: Reproducción de previews de canciones usando NAudio
- **Zona Pública**: Acceso a contenido sin necesidad de registro
- **Sistema de Logging**: Registro detallado de actividades usando Serilog

### 🛠️ Características Técnicas
- **Interfaz de Consola Moderna**: Interfaz atractiva usando Spectre.Console
- **Almacenamiento JSON**: Persistencia de datos en archivos JSON
- **Arquitectura Modular**: Separación clara entre modelos, servicios, vistas y utilidades
- **Manejo de Audio**: Integración con NAudio para reproducción de archivos MP3

## 🚀 Tecnologías Utilizadas

- **.NET 8.0** - Framework principal
- **NAudio** - Biblioteca para manejo de audio
- **Spectre.Console** - Interfaz de consola moderna y colorida
- **Serilog** - Sistema de logging avanzado
- **Microsoft.Extensions.Logging** - Framework de logging de Microsoft

## 🎮 Uso de la Aplicación

### Menú Principal
Al iniciar la aplicación, se presenta un menú principal con las siguientes opciones:

1. **🎧 Iniciar sesión** - Acceder con una cuenta existente
2. **📝 Registrarse** - Crear una nueva cuenta de usuario
3. **🌐 Zona pública** - Explorar contenido sin registro
4. **❌ Salir** - Cerrar la aplicación

### Funcionalidades por Módulo

#### Gestión de Usuarios
- Registro de nuevos usuarios
- Inicio de sesión
- Visualización y edición de perfiles

#### Catálogo Musical
- Exploración de artistas
- Navegación por álbumes
- Búsqueda de canciones
- Reproducción de previews

#### Listas de Reproducción
- Creación de playlists personalizadas
- Adición/eliminación de canciones
- Gestión de playlists existentes

## 📊 Almacenamiento de Datos

La aplicación utiliza archivos JSON para persistir los datos:

- **`datos.json`** - Información de artistas, álbumes y canciones
- **`UserData.json`** - Datos de usuarios registrados
- **`Data/logs/`** - Archivos de log de la aplicación

## 🎵 Archivos de Audio

Los archivos de muestra se almacenan en la carpeta `Previews/` y incluyen:
- Previews de canciones en formato MP3
- Archivos de muestra para demostración

## 📝 Logging

La aplicación implementa un sistema de logging completo usando Serilog que registra:
- Inicio y cierre de la aplicación
- Carga de datos
- Acciones de usuario
- Errores y excepciones

Los logs se almacenan en `Data/logs/` con formato de fecha.

## 🐳 Ejecución con Docker

### **Prerequisitos**
- Tener Docker instalado en tu sistema
- Conexión a internet para descargar la imagen

### **Opción 1: Usar imagen de Docker Hub (Recomendado)**

```bash
# 1. Descargar la imagen del proyecto
docker pull a27818/proyecto-aa1:latest

# 2. Ejecutar la aplicación
docker run -it --rm -v data:/app/data -p 7818:7818 a27818/proyecto-aa1:latest
```

### **Opción 2: Construir localmente desde el código**

```bash
# 1. Clonar el repositorio
git clone https://github.com/Aidanasv/proyecto-aa1.git
cd proyecto-aa1

# 2. Construir la imagen
docker build -t proyecto-aa1 .

# 3. Ejecutar
docker run -it --rm -v data:/app/data -p 7818:7818 proyecto-aa1
```

### **Explicación de parámetros:**
- `-it`: Modo interactivo para aplicación de consola
- `--rm`: Elimina el contenedor al terminar
- `-v data:/app/data`: Persiste los datos entre ejecuciones
- `-p 7818:7818`: Expone el puerto 7818

