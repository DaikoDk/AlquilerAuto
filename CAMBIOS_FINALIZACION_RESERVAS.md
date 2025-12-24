# Implementación de Finalización de Reservas

## Resumen de Cambios

Se ha implementado un sistema completo para finalizar reservas cuando están próximas a su fecha de fin (2 horas o menos), incluyendo:

### 1. Nuevo ViewModel: `FinalizarReservaVM.cs`
- **Ubicación**: `AlquilerAuto\ViewModels\FinalizarReservaVM.cs`
- **Propósito**: Manejar los datos para finalizar una reserva
- **Campos principales**:
  - `KilometrajeFin`: Kilometraje final del vehículo (requerido)
  - `EstadoEntrega`: Estado del vehículo (Óptimo, Con daños reparados, Pendiente reparación)
  - `Observaciones`: Notas sobre el estado del vehículo y daños

### 2. Nuevas Vistas

#### `Finalizar.cshtml`
- **Ubicación**: `AlquilerAuto\Views\Reserva\Finalizar.cshtml`
- **Características**:
  - Formulario para ingresar kilometraje final
  - Selector de estado de entrega del vehículo
  - Campo de observaciones para detallar daños o reparaciones
  - Muestra información completa de la reserva
  - Muestra datos del cliente y vehículo

#### `IniciarAlquiler.cshtml`
- **Ubicación**: `AlquilerAuto\Views\Reserva\IniciarAlquiler.cshtml`
- **Características**:
  - Vista para iniciar el alquiler de una reserva
  - Muestra información detallada antes de iniciar
  - Registra el kilometraje inicial automáticamente

### 3. Actualización del Controlador

#### `ReservaController.cs`
Nuevos métodos agregados:

1. **`IniciarAlquiler(int id)`** (GET)
   - Muestra la vista para iniciar el alquiler
   - Valida que la reserva esté en estado "Reservado"

2. **`IniciarAlquilerConfirmado(int id)`** (POST)
   - Ejecuta el inicio del alquiler
   - Cambia el estado a "Alquilado"
   - Registra el kilometraje inicial del vehículo

3. **`Finalizar(int id)`** (GET)
   - Muestra el formulario para finalizar la reserva
   - Valida que la reserva esté en estado "Alquilado"

4. **`FinalizarConfirmado(FinalizarReservaVM vm)`** (POST)
   - Procesa la finalización de la reserva
   - Valida el kilometraje final
   - Actualiza el estado del vehículo según el estado de entrega
   - Calcula automáticamente la mora si hay retraso
   - Suma costos de reparaciones si aplica

### 4. Actualización de la Vista Index

#### `Index.cshtml`
Se modificó la lógica de visualización de acciones:

**Para reservas en estado "Reservado":**
- Botón "Iniciar" (azul) - Inicia el alquiler
- Botón "Cancelar" (rojo) - Cancela la reserva

**Para reservas en estado "Alquilado":**
- **Si faltan 2 horas o menos para finalizar:**
  - Botón "Finalizar" (verde) - Permite completar la reserva con kilometraje y observaciones
  - Se muestra badge "? Por finalizar"
- **Si faltan más de 2 horas:**
  - Botón "Cancelar" (rojo) - Permite cancelar

**Para reservas "Finalizadas" o "Canceladas":**
- Sin acciones disponibles (solo consulta)

### 5. Actualización de la Vista Cancelar

#### `Cancelar.cshtml`
- Se agregó advertencia visual cuando la reserva está próxima a finalizar
- Se sugiere usar "Finalizar" en lugar de "Cancelar"
- Se muestra un botón alternativo "Mejor Finalizar" cuando aplica

## Lógica de Negocio

### Cálculo de Tiempo Restante
```csharp
var fechaHoraFin = item.fechaFin.Add(item.horaFin);
var ahora = DateTime.Now;
var horasRestantes = (fechaHoraFin - ahora).TotalHours;
var puedeFinalizarse = horasRestantes <= 2 && item.estado == "Alquilado";
```

### Flujo de Estados de una Reserva
1. **Reservado** ? (Iniciar) ? **Alquilado**
2. **Alquilado** ? (Finalizar) ? **Finalizado**
3. **Reservado/Alquilado** ? (Cancelar) ? **Cancelado**

### Validaciones en Finalización
- El kilometraje final debe ser mayor o igual al kilometraje inicial
- Solo se pueden finalizar reservas en estado "Alquilado"
- Se calcula automáticamente:
  - **Mora**: Si hay retraso (según configuración del sistema)
  - **Costo de reparaciones**: Si el cliente es responsable de daños
  - **Total**: Subtotal + Mora + Reparaciones

### Estados de Entrega del Vehículo
1. **Óptimo**: Sin daños - El auto queda "Disponible"
2. **Con daños reparados**: Reparaciones completadas - El auto queda "Disponible"
3. **Pendiente de reparación**: Requiere reparación - El auto queda "En reparacion"

## Procedimientos Almacenados Utilizados

- `usp_reserva_iniciar_alquiler`: Inicia el alquiler (cambia estado y registra kilometraje inicial)
- `usp_reserva_finalizar`: Finaliza la reserva (calcula mora, actualiza kilometraje, cambia estado del auto)
- `usp_reserva_buscar_detalle`: Obtiene detalles completos de una reserva

## Características Adicionales

### Cálculo Automático de Mora
El stored procedure `usp_reserva_finalizar` calcula automáticamente:
- Tiempo de gracia (configurado en `tb_configuracion`)
- Mora por hora (configurado en `tb_configuracion`)
- Redondeo hacia arriba de horas de retraso

### Registro de Kilometraje
- Se registra en `tb_historial_kilometraje` cada vez que se finaliza una reserva
- Permite tracking del uso de cada vehículo

### Actualización del Estado del Auto
Según el estado de entrega:
- **Óptimo/Con daños reparados** ? Auto queda "Disponible"
- **Pendiente reparación** ? Auto queda "En reparacion"

## Interfaz de Usuario

### Mejoras Visuales
- Badge "? Por finalizar" en reservas próximas a terminar
- Botones con iconos descriptivos:
  - ?? Iniciar (azul)
  - ? Finalizar (verde)
  - ? Cancelar (rojo)
- Alertas informativas con instrucciones claras
- Formularios con validación en cliente y servidor

### Responsive Design
- Layout de 2 columnas en desktop
- Adaptable a móviles
- Uso de Bootstrap 5 para diseño consistente

## Pruebas Sugeridas

1. **Crear una reserva** con fecha/hora de fin próxima (menos de 2 horas)
2. **Iniciar el alquiler** y verificar que cambie a estado "Alquilado"
3. **Verificar** que aparezca el botón "Finalizar" en lugar de "Cancelar"
4. **Finalizar la reserva** ingresando:
   - Kilometraje final mayor al inicial
   - Estado de entrega
   - Observaciones sobre el estado del vehículo
5. **Verificar** que se calculen correctamente:
   - Mora (si hay retraso)
   - Total final
   - Estado del auto actualizado

## Archivos Modificados

- ? `AlquilerAuto\Controllers\ReservaController.cs` (actualizado)
- ? `AlquilerAuto\Views\Reserva\Index.cshtml` (actualizado)
- ? `AlquilerAuto\Views\Reserva\Cancelar.cshtml` (actualizado)

## Archivos Nuevos

- ? `AlquilerAuto\ViewModels\FinalizarReservaVM.cs` (nuevo)
- ? `AlquilerAuto\Views\Reserva\Finalizar.cshtml` (nuevo)
- ? `AlquilerAuto\Views\Reserva\IniciarAlquiler.cshtml` (nuevo)

## Notas Importantes

- La validación de "2 horas o menos" se hace en la vista para decisión de UI
- La lógica de negocio en el backend no restringe la finalización por tiempo
- Los stored procedures ya existentes manejan toda la lógica compleja (mora, reparaciones, etc.)
- El sistema respeta la configuración en `tb_configuracion` para moras y tiempos de gracia
