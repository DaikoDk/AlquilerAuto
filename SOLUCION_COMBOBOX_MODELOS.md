# Solución: Combobox de Modelos no carga al seleccionar Marca

## ?? Problema

Al seleccionar una marca en el formulario de Auto, el combobox de modelos no se carga y permanece con el mensaje "Seleccione primero una marca".

## ?? Causa Raíz

El problema estaba en la ubicación del script JavaScript. El código jQuery estaba dentro de una **`@section Scripts`** en el partial view **`_PartialAutoForm.cshtml`**.

### ¿Por qué no funcionaba?

En ASP.NET Core MVC/Razor Pages:
- Las **secciones (`@section Scripts`)** solo se renderizan en las **vistas principales** (Views) y en **Layouts**
- Las secciones en **Partial Views** **NO se ejecutan automáticamente**
- El script estaba "oculto" dentro del partial y nunca se cargaba en el navegador

## ? Solución Implementada

### 1. Mover el Script a las Vistas Principales

#### **Create.cshtml**
```razor
@section Scripts {
    @{ await Html.RenderPartialAsync("_ValidationScriptsPartial"); }
    
    <script>
        $(document).ready(function() {
            $('#selectMarca').change(function() {
                var idMarca = $(this).val();
                var $selectModelo = $('#selectModelo');
                
                $selectModelo.empty();
                $selectModelo.append('<option value="">Cargando...</option>');
                
                if (idMarca) {
                    $.ajax({
                        url: '@Url.Action("ObtenerModelosPorMarca", "Auto")',
                        type: 'GET',
                        data: { idMarca: idMarca },
                        success: function(modelos) {
                            $selectModelo.empty();
                            $selectModelo.append('<option value="">Seleccione un modelo</option>');
                            $.each(modelos, function(i, modelo) {
                                $selectModelo.append($('<option></option>').val(modelo.value).html(modelo.text));
                            });
                        },
                        error: function(xhr, status, error) {
                            console.error('Error al cargar modelos:', error);
                            $selectModelo.empty();
                            $selectModelo.append('<option value="">Error al cargar modelos</option>');
                        }
                    });
                } else {
                    $selectModelo.empty();
                    $selectModelo.append('<option value="">Seleccione primero una marca</option>');
                }
            });
        });
    </script>
}
```

#### **Edit.cshtml**
Además del script anterior, se agregó:
```javascript
// Cargar modelos al cargar la página si hay una marca seleccionada
var marcaActual = $('#selectMarca').val();
if (marcaActual) {
    $('#selectMarca').trigger('change');
}
```

### 2. Limpiar el Partial

Se eliminó la sección `@section Scripts` del `_PartialAutoForm.cshtml` ya que no se ejecutaba.

## ?? Archivos Modificados

- ? `AlquilerAuto\Views\Auto\Create.cshtml` - Agregado script de carga de modelos
- ? `AlquilerAuto\Views\Auto\Edit.cshtml` - Agregado script con auto-carga para edición
- ? `AlquilerAuto\Views\Shared\_PartialAutoForm.cshtml` - Eliminada sección Scripts inútil

## ?? Funcionamiento Correcto

### Flujo en Create (Crear Auto)

1. Usuario selecciona una **Marca** del dropdown
2. Se dispara el evento `change` de jQuery
3. Se hace una llamada AJAX a `/Auto/ObtenerModelosPorMarca?idMarca=X`
4. El controlador devuelve la lista de modelos en formato JSON
5. jQuery actualiza dinámicamente el dropdown de **Modelos**
6. Usuario puede seleccionar el modelo correspondiente

### Flujo en Edit (Editar Auto)

1. La página carga con una marca ya seleccionada
2. El script detecta automáticamente la marca actual
3. Dispara el evento `change` para cargar los modelos
4. Los modelos se cargan y se pre-selecciona el modelo actual del auto

## ?? Endpoint del Controlador

```csharp
[HttpGet]
public JsonResult ObtenerModelosPorMarca(int idMarca)
{
    var modelos = _modeloDAO.ListadoPorMarca(idMarca)
        .Select(m => new SelectListItem
        {
            Value = m.idModelo.ToString(),
            Text = m.nombre
        })
        .ToList();

    return Json(modelos);
}
```

## ?? Lecciones Aprendidas

### ? NO hacer:
- Poner `@section Scripts` en Partial Views
- Asumir que los scripts en partials se ejecutan automáticamente

### ? SÍ hacer:
- Colocar scripts en las vistas principales que usan el partial
- Usar eventos de jQuery para interactividad
- Agregar `console.error` para debugging
- Documentar comportamientos dinámicos

## ?? Cómo Probar

1. Ir a **Autos** ? **Nuevo Auto**
2. Seleccionar una marca (ej: "Toyota")
3. Verificar que el dropdown de Modelos se llena automáticamente
4. Seleccionar un modelo (ej: "Corolla")
5. Completar el resto del formulario
6. Guardar

### Debug (si aún no funciona):

1. Presionar **F12** para abrir DevTools
2. Ir a la pestaña **Console**
3. Buscar errores de JavaScript
4. Ir a la pestaña **Network**
5. Buscar la llamada a `ObtenerModelosPorMarca`
6. Verificar que devuelva datos JSON correctos

## ?? Ejemplo de Respuesta JSON

```json
[
    {
        "value": "1",
        "text": "Yaris"
    },
    {
        "value": "2",
        "text": "Corolla"
    },
    {
        "value": "3",
        "text": "RAV4"
    }
]
```

---

**Fecha de corrección:** 22/12/2025  
**Problema:** Scripts en Partial Views no se ejecutan  
**Solución:** Mover scripts a vistas principales
