# Guía de Uso: Sistema de Finalización de Reservas

## ?? Resumen
Se ha implementado la funcionalidad para finalizar reservas cuando están próximas a completarse (2 horas o menos), permitiendo registrar el kilometraje final, el estado del vehículo y observaciones sobre daños o reparaciones.

## ?? Flujo de Uso

### 1. Crear una Reserva
- Navega a **Reservas** ? **Nueva Reserva**
- Selecciona cliente y vehículo
- Define fechas y horas de inicio/fin
- El sistema calculará automáticamente el costo

### 2. Iniciar el Alquiler
- En el listado de reservas, localiza la reserva en estado **"Reservado"**
- Haz clic en el botón **"Iniciar"** (azul)
- Revisa los detalles y confirma
- El sistema:
  - Cambiará el estado a **"Alquilado"**
  - Registrará el kilometraje inicial del vehículo
  - Cambiará el estado del auto a **"Alquilado"**

### 3. Finalizar la Reserva

#### Cuándo aparece el botón "Finalizar"
El botón **"Finalizar"** (verde) aparece cuando:
- La reserva está en estado **"Alquilado"**
- **Y** faltan **2 horas o menos** para la fecha/hora de fin programada

#### Proceso de Finalización
1. Haz clic en **"Finalizar"**
2. Completa el formulario:
   - **Kilometraje Final**: Debe ser mayor o igual al kilometraje inicial
   - **Estado de Entrega**: Selecciona una opción:
     - ? **Óptimo**: Sin daños (el auto quedará disponible)
     - ?? **Con daños reparados**: Ya reparado (el auto quedará disponible)
     - ?? **Pendiente de reparación**: Requiere reparación (el auto quedará en reparación)
   - **Observaciones**: Describe el estado del vehículo, daños encontrados, etc.
3. Haz clic en **"Finalizar Reserva"**

#### Qué hace el sistema automáticamente:
- ? Calcula la **mora** si hay retraso (según configuración)
- ? Suma **costos de reparaciones** si el cliente es responsable
- ? Calcula el **total final** (Subtotal + Mora + Reparaciones)
- ? Actualiza el **kilometraje del vehículo**
- ? Registra el **historial de kilometraje**
- ? Cambia el **estado del vehículo** según el estado de entrega
- ? Actualiza **contadores del cliente** (incidentes si hay reparaciones)

### 4. Cancelar una Reserva

#### Cuándo aparece el botón "Cancelar"
- Para reservas en estado **"Reservado"** (siempre)
- Para reservas en estado **"Alquilado"** que falten **más de 2 horas** para finalizar

#### Si intentas cancelar cerca de la finalización:
- El sistema te mostrará una advertencia
- Te sugerirá usar **"Finalizar"** en lugar de **"Cancelar"**
- Aparecerá un botón alternativo **"Mejor Finalizar"**

## ?? Interfaz Visual

### Estados de Reserva
- ?? **Reservado** (badge azul) - Reserva creada, pendiente de iniciar
- ?? **Alquilado** (badge amarillo) - Alquiler en progreso
- ?? **Finalizado** (badge verde) - Completado exitosamente
- ? **Cancelado** (badge gris) - Cancelado

### Indicadores Especiales
- ? **Por finalizar** - Aparece cuando faltan 2 horas o menos
- Este badge ayuda a identificar rápidamente qué reservas están por completarse

## ?? Casos de Uso Comunes

### Caso 1: Reserva con devolución a tiempo
1. Cliente reserva auto del 15/12 09:00 al 20/12 18:00
2. Se inicia el alquiler el 15/12
3. A partir del 20/12 a las 16:00 (2 horas antes), aparece botón "Finalizar"
4. Se finaliza con estado "Óptimo"
5. Total = Subtotal (sin mora)

### Caso 2: Reserva con retraso
1. Reserva debía terminar 20/12 18:00
2. Cliente devuelve el auto el 20/12 20:30 (2.5 horas tarde)
3. Al finalizar, el sistema calcula mora automáticamente
4. Si hay 30 min de gracia, cobra mora por 2 horas
5. Total = Subtotal + Mora

### Caso 3: Reserva con daños
1. Al finalizar, se detectan daños en el vehículo
2. Se selecciona "Pendiente de reparación"
3. Se describe el daño en observaciones
4. El auto queda en estado "En reparación"
5. Se pueden agregar reparaciones desde el sistema
6. Total = Subtotal + Mora + Costo Reparaciones

## ?? Configuración del Sistema

Las siguientes configuraciones afectan el cálculo de mora:
- **MORA_POR_HORA**: Monto a cobrar por cada hora de retraso
- **TIEMPO_GRACIA_MINUTOS**: Minutos de tolerancia antes de cobrar mora
- **MORA_POR_DIA**: Monto de mora por día (alternativa)

Estas configuraciones se encuentran en la tabla `tb_configuracion` de la base de datos.

## ?? Validaciones Implementadas

### Al Iniciar Alquiler:
- ? Solo se pueden iniciar reservas en estado "Reservado"
- ? No se puede iniciar antes de la fecha programada

### Al Finalizar:
- ? Solo se pueden finalizar reservas en estado "Alquilado"
- ? El kilometraje final debe ser ? kilometraje inicial
- ? Todos los campos del formulario son obligatorios
- ? Las observaciones tienen un máximo de 500 caracteres

### Al Cancelar:
- ? Solo se pueden cancelar reservas "Reservado" o "Alquilado"
- ? No se pueden cancelar reservas "Finalizadas"
- ? Se sugiere finalizar si está cerca del tiempo límite

## ?? Reportes y Consultas

Después de finalizar una reserva, puedes:
- Ver el detalle completo en el listado
- Consultar el historial de kilometraje del vehículo
- Ver las reparaciones asociadas (si las hay)
- Consultar el historial del cliente

## ?? Resolución de Problemas

### "El kilometraje final no puede ser menor al inicial"
- Verifica que hayas ingresado el kilometraje correcto
- El kilometraje inicial se registró al iniciar el alquiler

### "Solo se pueden finalizar reservas en estado Alquilado"
- Asegúrate de haber iniciado el alquiler primero
- Verifica el estado actual de la reserva

### No aparece el botón "Finalizar"
- Verifica que la reserva esté en estado "Alquilado"
- Confirma que falten 2 horas o menos para la fecha/hora de fin
- Si falta más tiempo, aparecerá el botón "Cancelar" en su lugar

## ?? Buenas Prácticas

1. **Inicia el alquiler** cuando el cliente recoja el vehículo
2. **Verifica el estado del vehículo** antes de entregarlo
3. **Registra el kilometraje exacto** al finalizar
4. **Describe detalladamente** cualquier daño en las observaciones
5. **Selecciona el estado de entrega correcto** para que el auto tenga la disponibilidad adecuada
6. **Documenta reparaciones pendientes** para seguimiento posterior

## ?? Integración con Otros Módulos

- **Módulo de Autos**: Actualiza automáticamente el estado y kilometraje
- **Módulo de Clientes**: Actualiza contadores de reservas e incidentes
- **Módulo de Reparaciones**: Registra reparaciones pendientes
- **Historial**: Mantiene registro de todos los movimientos de kilometraje

---

**Nota**: Todos los cálculos de mora y costos se realizan automáticamente según la configuración del sistema y la información almacenada en la base de datos.
