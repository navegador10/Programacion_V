# Actividad 1. Desafío Git: trazabilidad y evolución de una API

**Estudiante:** Adelson Aguirre Rodríguez

**API base:** `ProgramacionV.Api` — API de Gestión Académica construida en el Laboratorio 0 (ASP.NET Core 10, SQLite, Entity Framework Core, Repository y Scalar), tal como se trabajó durante las sesiones del curso.

---

## Desafío 1. Construcción del historial de ramas

**Comandos ejecutados, en orden:**

```bash
git init
git add -A
git commit -m "Estructura base de la API de gestion academica (Laboratorio 0)" # C1 en main

git branch feature/telefono-estudiante
git branch feature/consulta-estudiantes
git switch feature/telefono-estudiante
git add -A
git commit -m "Agrega campo telefono al estudiante"                     # C2

git switch -c feature/validar-telefono          # creada desde feature/telefono-estudiante
git add -A
git commit -m "Agrega validacion de formato para telefono del estudiante" # C4

git switch feature/consulta-estudiantes          # creada desde main
git add -A
git commit -m "Agrega endpoint de consulta de estudiantes por programa academico" # C3

# Push del repositorio a GitHub:
git remote add origin https://github.com/navegador10/Programacion_V.git
git push -u origin main feature/telefono-estudiante feature/consulta-estudiantes feature/validar-telefono feature/cambio-prueba
```

**Resultado del comando de visualización del árbol:**

```
$ git log --all --graph --oneline --decorate

*   3f69140 (HEAD -> main) Integra feature/telefono-estudiante a main
|\
| | * 2c0e896 (feature/cambio-prueba) Agrega version de la API en el arranque
| |/
|/|
| | * b95f307 (feature/consulta-estudiantes) Agrega endpoint de consulta de estudiantes por programa academico
| |/
|/|
| | * 40b0211 (feature/validar-telefono) Agrega validacion de formato para telefono del estudiante
| |/
| * a3da2db (feature/telefono-estudiante) Agrega campo telefono al estudiante
|/
* 6c2d16a Estructura base de la API de gestion academica (Laboratorio 0)
```

> El commit `3f69140` (fusión) y `2c0e896` (Desafío 4) se explican más abajo, en las secciones de *Integración de cambios* y *Desafío 4*; no forman parte de los 4 commits C1-C4 pedidos, que corresponden únicamente al historial de ramas del Desafío 1.

**Hash corto de cada commit y rama:**

| Commit | Hash corto | Rama |
|---|---|---|
| C1 | `6c2d16a` | `main` |
| C2 | `a3da2db` | `feature/telefono-estudiante` |
| C3 | `b95f307` | `feature/consulta-estudiantes` |
| C4 | `40b0211` | `feature/validar-telefono` |

**¿Por qué `feature/validar-telefono` se origina desde `feature/telefono-estudiante` y no directamente desde `main`?**

Porque la validación de formato del teléfono depende de que el campo `Telefono` ya exista en el modelo `Estudiante`. Ese campo se introdujo en el commit C2, dentro de `feature/telefono-estudiante`. Si `feature/validar-telefono` se hubiera creado desde `main`, no tendría ese campo disponible y la validación no tendría sobre qué aplicarse. Ramificar desde `feature/telefono-estudiante` permite construir sobre un trabajo que aún no ha sido integrado a `main`, manteniendo el historial coherente con la dependencia real entre ambas funcionalidades.

---

## Desafío 2. Investigación del historial

### 1-4. Commit donde se agregó el teléfono / autor / mensaje / fecha

**Comando utilizado:**

```bash
git show a3da2db --stat
```

**Resultado obtenido:**

```
commit a3da2dbc0b7a3b9d9be7195b3d4d05ed1d1702f7
Author: Adelson Aguirre Rodríguez <adelson.aguirre@gmail.com>
Date:   Sun Sep 13 13:54:28 2026 +0000

    Agrega campo telefono al estudiante

 Data/AppDbContext.cs                 | 5 +++++
 Models/Estudiante.cs                 | 1 +
 Repositories/EstudianteRepository.cs | 1 +
 3 files changed, 7 insertions(+)
```

**Respuesta:** El commit donde se agregó el teléfono es `a3da2db`. Fue realizado por Adelson Aguirre Rodríguez (`adelson.aguirre@gmail.com`), el domingo 13 de septiembre de 2026, con el mensaje "Agrega campo telefono al estudiante".

### 5. ¿Qué archivos fueron modificados en ese commit?

**Comando utilizado:**

```bash
git show --name-only a3da2db
```

**Resultado obtenido:**

```
Data/AppDbContext.cs
Models/Estudiante.cs
Repositories/EstudianteRepository.cs
```

**Respuesta:** Se modificaron tres archivos: `Models/Estudiante.cs` (se agregó la propiedad `Telefono`), `Repositories/EstudianteRepository.cs` (se incluyó `Telefono` en la actualización) y `Data/AppDbContext.cs` (se agregó el teléfono a los datos semilla de cada estudiante).

### 6. ¿Cuál fue el commit donde se implementó la validación del teléfono?

**Comando utilizado:**

```bash
git log --oneline --all --grep="validacion" -i
```

**Resultado obtenido:**

```
40b0211 Agrega validacion de formato para telefono del estudiante
```

**Respuesta:** El commit `40b0211`, en la rama `feature/validar-telefono`, fue donde se implementó la validación del formato del teléfono (creación de `Validators/TelefonoValidator.cs` y su uso en el controlador).

---

## Desafío 3. Recuperación de cambios

Contexto: ubicado en `feature/telefono-estudiante`, se modificó `Program.cs` sin agregarlo al staging ni convertirlo en commit.

**Comandos utilizados, en orden:**

```bash
git switch feature/telefono-estudiante
# (edición manual del archivo Program.cs)
git status
git diff
git restore Program.cs
git status
```

**Resultado antes de recuperar el archivo (`git status`):**

```
On branch feature/telefono-estudiante
Changes not staged for commit:
  (use "git add <file>..." to update what will be committed)
  (use "git restore <file>..." to discard changes in working directory)
        modified:   Program.cs

no changes added to commit (use "git add" and/or "git commit -a")
```

**Resultado del comando que muestra las diferencias (`git diff`):**

```diff
diff --git a/Program.cs b/Program.cs
index 145a07b..73d2b33 100644
--- a/Program.cs
+++ b/Program.cs
@@ -43,4 +43,4 @@ app.MapGet("/", () =>
 app.UseHttpsRedirection();
 app.MapControllers();
 
-app.Run();
+app.Run(); // cambio de prueba no deseado
```

**Comando utilizado para recuperar el archivo:**

```bash
git restore Program.cs
```

**Resultado final (`git status` después de recuperar):**

```
On branch feature/telefono-estudiante
nothing to commit, working tree clean
```

**Respuesta — ¿Qué habría ocurrido si el cambio ya hubiese sido incluido en un commit?**

`git restore` solo actúa sobre cambios que aún no han sido confirmados (no están en un commit); revierte el archivo a la última versión registrada en el historial. Si el cambio ya hubiera quedado dentro de un commit, `git restore` no serviría para deshacerlo, porque ese commit ya forma parte del historial. En ese caso sería necesario usar `git revert <hash>` (para crear un nuevo commit que anule los cambios, preservando el historial) o `git reset` (para mover el puntero de la rama antes de ese commit, reescribiendo el historial). La elección entre ambos depende de si el commit ya fue compartido con otras personas: `revert` es seguro incluso si el commit ya fue publicado, mientras que `reset` reescribe el historial y solo debería usarse en commits que aún no se han compartido.

---

## Desafío 4. Deshacer un commit

**Rama desde la cual se creó `feature/cambio-prueba`:** `main`

**Comandos utilizados, en orden:**

```bash
git switch main
git switch -c feature/cambio-prueba
# (modificación simple en Program.cs)
git add -A
git commit -m "Agrega nota de version pendiente"
git log --oneline -3
git reset --soft HEAD~1
git status
git log --oneline -3
# (corrección del cambio en Program.cs)
git add -A
git commit -m "Agrega version de la API en el arranque"
```

**Hash del commit original:**

```
1a5cd451a4e93173bf014cdb7abb66f96aba35b7   (corto: 1a5cd45)
```

**Historial antes de deshacer el commit:**

```
1a5cd45 Agrega nota de version pendiente
6c2d16a Estructura base de la API de gestion academica (Laboratorio 0)
```

**Comando utilizado para deshacerlo:**

```bash
git reset --soft HEAD~1
```

**Estado del repositorio después de la operación:**

```
On branch feature/cambio-prueba
Changes to be committed:
  (use "git restore --staged <file>..." to unstage)
        modified:   Program.cs

--- historial ---
6c2d16a Estructura base de la API de gestion academica (Laboratorio 0)
```

**Explicación — ¿qué ocurrió con el commit?**

El commit `1a5cd45` dejó de existir en el historial de la rama: el puntero de `feature/cambio-prueba` volvió a apuntar a `6c2d16a` (el commit anterior). `git reset --soft` no borra el commit del todo de inmediato (sigue siendo alcanzable brevemente por el reflog), pero la rama ya no lo referencia, por lo que para efectos prácticos el commit se deshizo.

**Explicación — ¿qué ocurrió con los archivos modificados?**

Se conservaron intactos. Con `--soft`, los cambios que estaban en ese commit quedaron en el área de staging ("Changes to be committed"), listos para corregirse y volver a confirmarse sin necesidad de rehacer la edición desde cero.

**Nuevo hash del commit (tras corregir y volver a confirmar):**

```
2c0e896d4f9ce42f54d971dfc693e613c19de958   (corto: 2c0e896)
```

**Pregunta de análisis — ¿Qué diferencia existiría si el commit ya hubiera sido publicado y compartido en GitHub?**

Si el commit ya se hubiera subido a GitHub y otra persona lo hubiera descargado, usar `git reset --soft` (o cualquier variante de reset) movería el puntero de la rama local a un punto anterior del historial, pero el commit seguiría existiendo en el repositorio remoto. Al intentar subir los cambios se produciría una divergencia entre el historial local y el remoto, y sería necesario forzar el push (`git push --force`), lo cual reescribe el historial compartido y puede causar que los demás colaboradores pierdan referencias válidas o tengan conflictos al sincronizar. En un escenario colaborativo, la práctica recomendada en ese caso es usar `git revert` en lugar de `git reset`: `revert` crea un nuevo commit que deshace los cambios sin alterar ni eliminar el commit original, por lo que es seguro de publicar y no genera conflictos de historial para el resto del equipo.

---

## Integración de cambios (merge)

Aunque no es uno de los 4 desafíos puntuados, la guía general de la actividad pide demostrar comprensión de "la integración de cambios". Para cubrirlo explícitamente, se fusionó `feature/telefono-estudiante` hacia `main`.

**Comandos utilizados:**

```bash
git switch main
git merge feature/telefono-estudiante --no-ff -m "Integra feature/telefono-estudiante a main"
```

**Resultado obtenido:**

```
Merge made by the 'ort' strategy.
 Data/AppDbContext.cs                 | 5 +++++
 Models/Estudiante.cs                 | 1 +
 Repositories/EstudianteRepository.cs | 1 +
 3 files changed, 7 insertions(+)
```

**Historial resultante:**

```
*   3f69140 (HEAD -> main) Integra feature/telefono-estudiante a main
|\
| * a3da2db (feature/telefono-estudiante) Agrega campo telefono al estudiante
|/
* 6c2d16a Estructura base de la API de gestion academica (Laboratorio 0)
```

**Explicación:** Se usó `--no-ff` ("no fast-forward") intencionalmente: aunque Git podía integrar este cambio simplemente moviendo el puntero de `main` (fast-forward, sin generar un commit nuevo, porque `main` no tenía commits propios desde que se creó la rama), forzar un commit de fusión explícito (`3f69140`) deja evidencia clara en el historial de que ahí ocurrió una integración de una rama de feature hacia `main`, en lugar de que el cambio se vea como si siempre hubiera sido parte de `main`. No hubo conflictos porque `main` no había cambiado desde que `feature/telefono-estudiante` se creó a partir de él.
