# Corrección: Manejo de Reservas Vencidas

## Problema Identificado
Una reserva con fecha de fin 20/12/2025 18:00 en estado "Reservado" mostraba el botón "Iniciar" el 22/12/2025, cuando ya había vencido hace 2 días.

## Solución Implementada

### 1. Lógica de Botones Actualizada en `Index.cshtml`

La nueva lógica diferencia estos casos:

#### **Para estado "Reservado":**
- ? **Si ya pasó la fecha de fin** ? Solo muestra botón "Cancelar" (con badge "?? Vencida")
- ? **Si aún no pasó la fecha de fin** ? Muestra botones "Iniciar" + "Cancelar"

#### **Para estado "Alquilado":**
- ? **Si ya pasó la fecha de fin** ? Muestra botón "Finalizar" (con badge "?? Vencida")
- ? **Si faltan 2 horas o menos** ? Muestra botón "Finalizar" (con badge "? Por finalizar")
- ? **Si faltan más de 2 horas** ? Muestra botón "Cancelar"

### 2. Validación Agregada en el Controlador

En `ReservaController.cs` método `IniciarAlquiler`:
```csharp
// Validar que la reserva no esté vencida
var fechaHoraFin = detalle.FechaFin.Add(detalle.HoraFin);
if (DateTime.Now > fechaHoraFin)
{
    TempData["error"] = "No se puede iniciar una reserva vencida. Por favor, cancélela.";
    return RedirectToAction("Index");
}
```

### 3. Nuevas Variables Calculadas

En la vista Index.cshtml:
```csharp
var fechaHoraInicio = item.fechaInicio.Add(item.horaInicio);
var fechaHoraFin = item.fechaFin.Add(item.horaFin);
var ahora = DateTime.Now;
var horasRestantes = (fechaHoraFin - ahora).TotalHours;
var yaPasoFechaFin = ahora > fechaHoraFin;
var yaPasoFechaInicio = ahora > fechaHoraInicio;
```

## Matriz de Acciones según Estado y Tiempo

| Estado | Condición | Botón Mostrado | Badge |
|--------|-----------|----------------|-------|
| Reservado | Fecha fin no pasó | Iniciar + Cancelar | - |
| Reservado | Fecha fin ya pasó | Solo Cancelar | ?? Vencida |
| Alquilado | Fecha fin ya pasó | Finalizar | ?? Vencida |
| Alquilado | Faltan ?2 horas | Finalizar | ? Por finalizar |
| Alquilado | Faltan >2 horas | Cancelar | - |
| Finalizado | - | - | - |
| Cancelado | - | - | - |

## Ejemplo del Caso Reportado

**Situación:**
- Fecha actual: 22/12/2025
- Reserva: 15/12/2025 09:00 ? 20/12/2025 18:00
- Estado: "Reservado"

**Antes:**
- ? Mostraba: "Iniciar" + "Cancelar"
- ? Problema: No tiene sentido iniciar una reserva vencida

**Después:**
- ? Muestra: Solo "Cancelar"
- ? Badge: "?? Vencida"
- ? Mensaje: Si intenta iniciar por URL, recibe error

## Recomendaciones para el Caso de Uso Real

### Si la reserva está en "Reservado" y ya venció:
1. **Cancelar la reserva** para liberar el vehículo
2. Revisar por qué no se inició a tiempo
3. Crear una nueva reserva si el cliente aún necesita el auto

### Si la reserva está en "Alquilado" y ya venció:
1. **Finalizar inmediatamente** con los datos actuales
2. El sistema calculará automáticamente la mora por el retraso
3. Ingresar el kilometraje real actual del vehículo
4. Registrar cualquier daño en las observaciones

## Flujo Correcto

```
???????????????
?  Reservado  ?
???????????????
       ?
       ?? (Si venció) ? Cancelar
       ?
       ?? (Si no venció) ? Iniciar ? Alquilado
                                      ?
                                      ?? (Si venció) ? Finalizar
                                      ?? (?2 horas) ? Finalizar
                                      ?? (>2 horas) ? Cancelar
```

## Archivos Modificados

- ? `AlquilerAuto\Views\Reserva\Index.cshtml` - Lógica de botones corregida
- ? `AlquilerAuto\Controllers\ReservaController.cs` - Validación de reservas vencidas

## Notas Importantes

1. **La fecha del sistema importa**: El sistema usa `DateTime.Now` para todas las comparaciones
2. **El estado es fundamental**: Una reserva "Reservado" nunca fue iniciada, "Alquilado" significa que el auto fue entregado
3. **Las reservas vencidas no se eliminan automáticamente**: Deben cancelarse manualmente
4. **La mora se calcula en la finalización**: El stored procedure `usp_reserva_finalizar` calcula la mora automáticamente

## Mejora Futura Sugerida

Considerar implementar un proceso automático que:
1. Marque como "Vencidas" las reservas en estado "Reservado" que pasaron su fecha de fin
2. Envíe notificaciones para reservas "Alquiladas" que estén vencidas
3. Genere reportes de reservas no finalizadas

---
**Fecha de corrección:** 22/12/2025
**Desarrollador:** Sistema AlquilerAuto
