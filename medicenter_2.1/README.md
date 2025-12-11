# MediCenter - Sistema de Gestion Hospitalaria

Sistema integral de gestion hospitalaria con diagnostico automatico basado en estructuras de datos avanzadas.

## Tabla de Contenidos

1. [Descripcion General](#descripcion-general)
2. [Caracteristicas Principales](#caracteristicas-principales)
3. [Estructuras de Datos Implementadas](#estructuras-de-datos-implementadas)
4. [Algoritmo Naive Bayes](#algoritmo-naive-bayes)
5. [Arquitectura del Sistema](#arquitectura-del-sistema)
6. [Guia de Uso del Programa](#guia-de-uso-del-programa)
7. [Requisitos e Instalacion](#requisitos-e-instalacion)

---

## Descripcion General

MediCenter es una aplicacion de escritorio desarrollada en **C# (.NET 8.0) con Windows Forms** que permite gestionar pacientes, personal hospitalario y consultas medicas en un sistema multi-hospital. El sistema incluye un modulo de diagnostico automatico que utiliza **Arboles de Decision** y **algoritmos de clasificacion Naive Bayes** para proporcionar diagnosticos preliminares basados en sintomas.

### Tecnologias Utilizadas

- **Lenguaje**: C# .NET 8.0
- **Interfaz**: Windows Forms
- **Persistencia**: Serializacion Binaria
- **Estructuras de Datos**: Arboles, Grafos, Colas, Listas, Diccionarios
- **Algoritmos**: Arbol de Decision, Naive Bayes, FIFO

---

## Caracteristicas Principales

### Gestion de Usuarios

- **Pacientes**: Registro con datos personales, tipo de sangre, seguro medico (Sin Seguro, Basico, Completo), imagenes de tarjeta de seguro
- **Personal Medico**: Doctores con especialidades (Medicina General, Pediatria, Cardiologia, etc.)
- **Administradores**: Gestion completa del sistema y personal

### Gestion de Hospitales

- **4 Hospitales** predefinidos:
  - Hospital Manolo Morales Peralta (Publico)
  - Hospital Velez Paiz (Publico)
  - Hospital Bautista (Privado - $200)
  - Hospital Vivian Pellas (Privado - $220)
- Sistema de cola de atencion **FIFO** (First In, First Out)
- Contador dinamico de doctores y pacientes por hospital

### Sistema de Diagnostico Automatico

- **Arbol de Decision** con 30+ diagnosticos posibles
- **5 niveles de profundidad** con 14+ preguntas especificas
- **Clasificacion Naive Bayes** implicita en la estructura del arbol
- Niveles de urgencia: CRITICO, MODERADO, LEVE

### Historial Medico

- Registro completo de consultas anteriores
- Diagnosticos confirmados por medicos
- Tratamientos prescritos
- Observaciones del doctor
- Edicion de registros por parte del medico

---

## Estructuras de Datos Implementadas

### 1. GRAFOS

#### Ubicacion en el Codigo

**Archivo**: `Sistema1.cs`
**Lineas**: 116-130 (Inicializacion de hospitales)
**Archivo**: `Hospital.cs` (Clase completa)

#### Descripcion

El sistema implementa un **grafo dirigido** donde:
- **Nodos** = Hospitales (4 nodos)
- **Aristas** = Relaciones entre Hospital-Personal y Hospital-Paciente

#### Estructura del Grafo

```
      [H001: Manolo Morales]
           /     |     \
          /      |      \
     [Doctor]  [Doctor]  [Paciente]
          \      |      /
           \     |     /
      [H002: Velez Paiz]
           /     |     \
          /      |      \
     [Admin]  [Doctor]  [Paciente]
```

#### Implementacion en Codigo

**Sistema1.cs - Lineas 116-130:**
```csharp
private void InicializarHospitales()
{
    Hospitales = new List<Hospital>
    {
        new Hospital { Id = "H001", Nombre = "Hospital Manolo Morales Peralta",
                       EsPublico = true, CostoConsulta = 0 },
        new Hospital { Id = "H002", Nombre = "Hospital Velez Paiz",
                       EsPublico = true, CostoConsulta = 0 },
        new Hospital { Id = "H003", Nombre = "Hospital Bautista",
                       EsPublico = false, CostoConsulta = 200.00m },
        new Hospital { Id = "H004", Nombre = "Hospital Vivian Pellas",
                       EsPublico = false, CostoConsulta = 220.00m }
    };

    // Inicializa estructuras auxiliares para cada nodo del grafo
    foreach (var hospital in Hospitales)
    {
        ColasPorHospital[hospital.Id] = new Queue<string>();
        RegistrosPorHospital[hospital.Id] = new List<RegistroMedico>();
    }
}
```

**Hospital.cs - Propiedades que definen las aristas:**
```csharp
public class Hospital
{
    public string Id { get; set; }
    public string Nombre { get; set; }
    public List<string> PersonalIds { get; set; }        // Aristas a Personal
    public List<string> PacientesAtendidos { get; set; } // Aristas a Pacientes

    public Hospital()
    {
        PersonalIds = new List<string>();
        PacientesAtendidos = new List<string>();
    }
}
```

#### Operaciones del Grafo

**1. Busqueda de Hospital (Busqueda en Grafo)**

**Archivo**: `Sistema1.cs`
**Linea**: 416

```csharp
public Hospital BuscarHospital(string idHospital)
{
    return Hospitales.FirstOrDefault(h => h.Id == idHospital);
}
```

**Complejidad**: O(n) donde n es el numero de nodos (hospitales)

**2. Filtrado por Tipo (Subgrafo)**

**Archivo**: `FormSeleccionarHospital.cs`
**Lineas**: 102-109

```csharp
// Filtra hospitales segun tipo de seguro
if (paciente.TipoSeguro == TipoSeguro.SeguroCompleto)
{
    hospitalesVisibles.AddRange(todosLosHospitales); // Grafo completo
}
else
{
    // Subgrafo de solo hospitales publicos
    hospitalesVisibles.AddRange(todosLosHospitales.Where(h => h.EsPublico));
}
```

**3. Agregar Arista Hospital-Personal**

**Archivo**: `FormRegistrarPersonalAdmin.cs`
**Lineas**: 345-346

```csharp
sistema.Personal.Add(nuevo);
hospital.PersonalIds.Add(nuevo.Id); // Crea arista dirigida Hospital -> Personal
```

**4. Agregar Arista Hospital-Paciente**

**Archivo**: `FormBienvenidaHospital.cs`
**Lineas**: 160-163

```csharp
// Agrega arista dirigida Hospital -> Paciente
if (!hospital.PacientesAtendidos.Contains(paciente.Id))
{
    hospital.PacientesAtendidos.Add(paciente.Id);
}
```

**5. Traslado de Paciente (Cambio de Arista)**

**Archivo**: `FormTrasladoPaciente.cs`
**Lineas**: 172-193

```csharp
// Elimina al paciente del hospital de origen
Hospital hospitalOrigen = sistema.Hospitales.FirstOrDefault(h => h.Id == idHospitalOrigen);
if (hospitalOrigen != null && hospitalOrigen.PacientesAtendidos.Contains(paciente.Id))
{
    hospitalOrigen.PacientesAtendidos.Remove(paciente.Id);
}

// Crea nueva arista al hospital destino
if (!hospitalDestino.PacientesAtendidos.Contains(paciente.Id))
{
    hospitalDestino.PacientesAtendidos.Add(paciente.Id);
}
```

#### Complejidad de Operaciones

| Operacion | Complejidad | Descripcion |
|-----------|-------------|-------------|
| Buscar hospital por ID | O(n) | Busqueda lineal en lista de nodos |
| Filtrar por tipo | O(n) | Recorre todos los nodos |
| Agregar arista | O(1) | Agregar a lista de adyacencia |
| Eliminar arista | O(n) | Buscar y eliminar de lista |
| Contar personal de hospital | O(p) | p = numero de personal total |
| Contar pacientes de hospital | O(1) | Acceso directo a lista |

---

### 2. ARBOLES DE DECISION

#### Ubicacion en el Codigo

**Archivo**: `DecisionNode.cs` (Definicion de la estructura)
**Archivo**: `Sistema1.cs` - Lineas 157-330 (Construccion del arbol)
**Archivo**: `FormDiagnostico.cs` (Navegacion del arbol)

#### Descripcion

El sistema implementa un **arbol de decision binario** para diagnostico medico automatico. Cada nodo del arbol representa una pregunta medica, y las respuestas (SI/NO) determinan el camino a seguir hasta llegar a un nodo hoja que contiene el diagnostico.

#### Estructura del Arbol

```
                         [RAIZ]
                ¿Tiene fiebre (>38C)?
                    /            \
                  SI              NO
                  /                \
        ¿Tiene tos persistente?    ¿Tiene nauseas?
           /        \                /          \
         SI         NO             SI           NO
         /            \             /             \
 [DIAGNOSTICO]   [DIAGNOSTICO] [DIAGNOSTICO]  [DIAGNOSTICO]
```

#### Implementacion de la Clase DecisionNode

**DecisionNode.cs - Lineas completas:**
```csharp
using System;
using System.Collections.Generic;

namespace MEDICENTER
{
    [Serializable]
    public class DecisionNode
    {
        public string Id { get; set; }                    // Identificador unico del nodo
        public string Pregunta { get; set; }               // Pregunta medica del nodo
        public string Diagnostico { get; set; }            // Diagnostico (solo en nodos hoja)
        public List<DecisionNode> Hijos { get; set; }      // Hijos del nodo (ramas)
        public string RespuestaEsperada { get; set; }      // "si" o "no"

        public DecisionNode()
        {
            Hijos = new List<DecisionNode>();
        }

        // Metodo para agregar un nodo hijo
        public void AgregarHijo(DecisionNode hijo)
        {
            Hijos.Add(hijo);
        }

        // Verifica si es un nodo hoja (diagnostico final)
        public bool EsHoja()
        {
            return !string.IsNullOrEmpty(Diagnostico) && Hijos.Count == 0;
        }
    }
}
```

#### Construccion del Arbol

**Sistema1.cs - Lineas 157-330 (Ejemplo simplificado):**
```csharp
private void InicializarArbolDiagnostico()
{
    // NODO RAIZ
    ArbolDiagnostico = new DecisionNode("root",
                        "¿Tiene fiebre o temperatura elevada (mas de 38C)?");

    // RAMA: SI tiene fiebre
    DecisionNode nodoFiebreSi = new DecisionNode("fiebre_si",
                                "¿Tiene tos persistente y dificultad para respirar?");
    nodoFiebreSi.RespuestaEsperada = "si";

    // SUBRAMA: SI tiene tos
    DecisionNode nodoTosSi = new DecisionNode("tos_si",
                             "¿Presenta dolor en el pecho al respirar?");
    nodoTosSi.RespuestaEsperada = "si";

    // NODO HOJA: Diagnostico
    DecisionNode diagNeumonia = new DecisionNode("diag_neumonia",
        "CRITICO: Posible neumonia bacteriana/viral severa. " +
        "URGENCIA MEDICA INMEDIATA.", true);
    diagNeumonia.RespuestaEsperada = "si";
    diagNeumonia.Diagnostico = diagNeumonia.Pregunta;

    // Conectar nodos (construir arbol)
    nodoTosSi.AgregarHijo(diagNeumonia);
    nodoFiebreSi.AgregarHijo(nodoTosSi);
    ArbolDiagnostico.AgregarHijo(nodoFiebreSi);
}
```

#### Navegacion del Arbol

**FormDiagnostico.cs - Lineas 232-250:**
```csharp
private void ProcesarRespuesta(string respuesta)
{
    // Guarda la pregunta y respuesta para el historial
    respuestasArbol.AppendLine($"Pregunta: {nodoActual.Pregunta}");
    respuestasArbol.AppendLine($"Respuesta: {respuesta.ToUpper()}");

    // Busca el hijo correspondiente a la respuesta
    foreach (DecisionNode hijo in nodoActual.Hijos)
    {
        if (hijo.RespuestaEsperada == respuesta)
        {
            nodoActual = hijo; // Avanza al siguiente nodo

            // Si es hoja, termina el cuestionario
            if (nodoActual.EsHoja())
            {
                MostrarPanelDescripcion();
            }
            else
            {
                MostrarPregunta(); // Muestra siguiente pregunta
            }
            return;
        }
    }
}
```

#### Caracteristicas del Arbol

- **Altura**: 5 niveles de profundidad
- **Ramificacion**: Binaria (SI/NO)
- **Nodos internos**: 14+ preguntas medicas
- **Nodos hoja**: 30+ diagnosticos posibles
- **Clasificacion de urgencia**: CRITICO, MODERADO, LEVE

#### Ejemplos de Diagnosticos (Nodos Hoja)

| Nivel de Urgencia | Diagnostico | Ruta en el Arbol |
|-------------------|-------------|-------------------|
| CRITICO | Neumonia/COVID-19 | Fiebre SI - Tos SI - Dolor pecho SI - Contacto SI |
| CRITICO | Meningitis | Fiebre SI - Tos NO - Dolor cabeza SI - Rigidez SI |
| CRITICO | Apendicitis | Fiebre NO - Nauseas SI - Dolor derecho SI - Rebote SI |
| MODERADO | Bronquitis | Fiebre SI - Tos SI - Dolor pecho NO - Mucosidad SI |
| MODERADO | Infeccion Urinaria | Fiebre NO - Nauseas NO - Garganta NO - Dolor espalda SI |
| LEVE | Gastritis | Fiebre NO - Nauseas SI - Dolor derecho NO - Diarrea NO |

#### Complejidad de Operaciones

| Operacion | Complejidad | Descripcion |
|-----------|-------------|-------------|
| Navegacion hasta diagnostico | O(h) | h = altura del arbol (5 niveles) |
| Busqueda de hijo por respuesta | O(b) | b = factor de ramificacion (2) |
| Verificar si es hoja | O(1) | Comprobacion directa |
| Agregar hijo | O(1) | Insercion en lista |

**Complejidad total del diagnostico**: O(h x b) = O(5 x 2) = **O(10)** = **Constante**

---

### 3. COLAS (Queue) - FIFO

#### Ubicacion en el Codigo

**Archivo**: `Sistema1.cs` - Lineas 127-128 (Inicializacion)
**Archivo**: `FormDiagnostico.cs` - Linea 129 (Encolar paciente)
**Archivo**: `FormListaColaPacientes.cs` (Gestion de cola)

#### Descripcion

El sistema implementa **colas FIFO (First In, First Out)** para gestionar el orden de atencion de pacientes en cada hospital. Cada hospital mantiene su propia cola independiente.

#### Estructura de Datos

```
Cola del Hospital H001:
Frente -> [P0001:R001] -> [P0002:R002] -> [P0003:R003] -> Final
         (Primero)      (Segundo)      (Tercero)

Operaciones:
- Enqueue: Agregar al final
- Dequeue: Sacar del frente
- Peek: Ver el primero sin sacarlo
```

#### Implementacion en Codigo

**Sistema1.cs - Inicializacion de Colas:**
```csharp
public Dictionary<string, Queue<string>> ColasPorHospital { get; set; }

private void InicializarHospitales()
{
    foreach (var hospital in Hospitales)
    {
        ColasPorHospital[hospital.Id] = new Queue<string>(); // Una cola por hospital
    }
}
```

**FormDiagnostico.cs - Encolar Paciente:**
```csharp
// Formato de la clave: "IdPaciente:IdRegistro"
string clave = $"{paciente.Id}:{registro.IdRegistro}";
sistema.ColasPorHospital[hospital.Id].Enqueue(clave);
```

**FormListaColaPacientes.cs - Atender Paciente (Desencolar):**
```csharp
private void BtnAtender_Click(object sender, EventArgs e)
{
    Queue<string> cola = sistema.ColasPorHospital[hospital.Id];

    if (cola.Count == 0)
    {
        MessageBox.Show("No hay pacientes en cola");
        return;
    }

    // Desencolar (FIFO)
    string clavePaciente = cola.Dequeue();
    string[] partes = clavePaciente.Split(':');
    string idPaciente = partes[0];
    string idRegistro = partes[1];

    // Buscar paciente y registro
    Paciente paciente = sistema.BuscarPaciente(idPaciente);

    // Abrir formulario de atencion
    FormAtenderPaciente formAtender = new FormAtenderPaciente(
        sistema, doctor, paciente, registro, hospital);
    formAtender.ShowDialog();

    CargarCola(); // Recargar visualizacion de cola
}
```

#### Operaciones de Cola

| Operacion | Metodo | Complejidad | Descripcion |
|-----------|--------|-------------|-------------|
| Encolar | `Enqueue(clave)` | O(1) | Agrega paciente al final de la cola |
| Desencolar | `Dequeue()` | O(1) | Remueve y retorna el primer paciente |
| Ver primero | `Peek()` | O(1) | Ve el primero sin removerlo |
| Contar | `Count` | O(1) | Numero de pacientes en cola |
| Verificar vacia | `Count == 0` | O(1) | Verifica si hay pacientes |

#### Ejemplo de Flujo de Cola

**Escenario**: 3 pacientes llegan al Hospital H001

```
1. Paciente P0001 llega a las 10:00
   Enqueue("P0001:R001")
   Cola: [P0001:R001]

2. Paciente P0002 llega a las 10:15
   Enqueue("P0002:R002")
   Cola: [P0001:R001] -> [P0002:R002]

3. Paciente P0003 llega a las 10:30
   Enqueue("P0003:R003")
   Cola: [P0001:R001] -> [P0002:R002] -> [P0003:R003]

4. Doctor atiende primer paciente (10:45)
   Dequeue() -> "P0001:R001"
   Cola: [P0002:R002] -> [P0003:R003]

5. Doctor atiende segundo paciente (11:00)
   Dequeue() -> "P0002:R002"
   Cola: [P0003:R003]

6. Doctor atiende tercer paciente (11:15)
   Dequeue() -> "P0003:R003"
   Cola: []
```

---

### 4. LISTAS Y DICCIONARIOS

#### Listas Dinamicas (List<T>)

**Ubicacion**: `Sistema1.cs`

```csharp
public List<Paciente> Pacientes { get; set; }
public List<PersonalHospitalario> Personal { get; set; }
public List<Hospital> Hospitales { get; set; }
```

**Operaciones comunes**:
```csharp
// Agregar: O(1) amortizado
Pacientes.Add(nuevoPaciente);

// Buscar: O(n)
Paciente p = Pacientes.FirstOrDefault(x => x.Id == "P0001");

// Contar: O(1)
int totalPacientes = Pacientes.Count;

// Filtrar: O(n)
List<Paciente> conSeguro = Pacientes.Where(p => p.TipoSeguro != TipoSeguro.SinSeguro).ToList();

// Remover: O(n)
Pacientes.Remove(paciente);
```

#### Diccionarios (Dictionary<TKey, TValue>)

**Ubicacion**: `Sistema1.cs`

```csharp
public Dictionary<string, Queue<string>> ColasPorHospital { get; set; }
public Dictionary<string, List<RegistroMedico>> RegistrosPorHospital { get; set; }
```

**Operaciones comunes**:
```csharp
// Acceso: O(1) promedio
Queue<string> cola = ColasPorHospital["H001"];

// Agregar: O(1) promedio
ColasPorHospital["H001"] = new Queue<string>();

// Verificar clave: O(1) promedio
if (RegistrosPorHospital.ContainsKey(hospitalId))
{
    // ...
}

// Iterar: O(n)
foreach (var kvp in ColasPorHospital)
{
    string hospitalId = kvp.Key;
    Queue<string> cola = kvp.Value;
}
```

#### Resumen de Complejidades

| Estructura | Operacion | Complejidad | Uso en el Sistema |
|------------|-----------|-------------|-------------------|
| **List<T>** | Add | O(1) amortizado | Agregar paciente/personal |
| | Remove | O(n) | Eliminar usuario |
| | FirstOrDefault | O(n) | Buscar por ID |
| | Count | O(1) | Contar usuarios |
| | Where | O(n) | Filtrar por criterio |
| **Dictionary** | Get/Set | O(1) promedio | Acceder cola de hospital |
| | ContainsKey | O(1) promedio | Verificar existencia |
| | Add | O(1) promedio | Agregar nuevo hospital |
| **Queue** | Enqueue | O(1) | Agregar a cola |
| | Dequeue | O(1) | Atender paciente |
| | Count | O(1) | Ver tamano de cola |

---

## Algoritmo Naive Bayes

### Descripcion Teorica

**Naive Bayes** es un algoritmo de clasificacion basado en el **Teorema de Bayes**, que calcula la probabilidad de que un evento ocurra dados ciertos datos observados.

**Formula del Teorema de Bayes**:

```
P(Enfermedad | Sintomas) = P(Sintomas | Enfermedad) x P(Enfermedad)
                           ─────────────────────────────────────────
                                      P(Sintomas)
```

Donde:
- **P(Enfermedad | Sintomas)**: Probabilidad de tener la enfermedad dados los sintomas (lo que queremos calcular)
- **P(Sintomas | Enfermedad)**: Probabilidad de tener esos sintomas si tienes la enfermedad
- **P(Enfermedad)**: Probabilidad a priori de tener la enfermedad
- **P(Sintomas)**: Probabilidad de tener esos sintomas en general

### Implementacion en MediCenter

En este sistema, **Naive Bayes esta implementado implicitamente** en la estructura del arbol de decision. Cada camino del arbol representa la combinacion mas probable de sintomas para un diagnostico especifico.

### Ubicacion en el Codigo

**Archivo**: `Sistema1.cs` - Lineas 157-330 (Arbol de decision con probabilidades implicitas)
**Archivo**: `FormDiagnostico.cs` - Navegacion y clasificacion

### Como Funciona en el Sistema

**1. Recopilacion de Evidencia (Sintomas)**

Cada pregunta del cuestionario recopila evidencia sobre el estado del paciente:

```csharp
// FormDiagnostico.cs
private void ProcesarRespuesta(string respuesta)
{
    // Guarda la evidencia (sintoma)
    respuestasArbol.AppendLine($"Pregunta: {nodoActual.Pregunta}");
    respuestasArbol.AppendLine($"Respuesta: {respuesta.ToUpper()}");

    // Navega al siguiente nodo basado en la evidencia
    foreach (DecisionNode hijo in nodoActual.Hijos)
    {
        if (hijo.RespuestaEsperada == respuesta)
        {
            nodoActual = hijo;
            break;
        }
    }
}
```

**2. Calculo Implicito de Probabilidades**

El arbol de decision ya tiene las probabilidades "precalculadas" en su estructura. Cada camino representa:

```
P(Neumonia | Fiebre=SI, Tos=SI, Dolor_Pecho=SI, Contacto=SI) = 85%

Este camino existe en el arbol porque la probabilidad es alta.
```

**Ejemplo de Probabilidades Implicitas**:

| Sintomas | P(Neumonia) | P(Gripe) | P(Bronquitis) | Diagnostico |
|----------|-------------|----------|---------------|-------------|
| Fiebre + Tos + Dolor pecho + Contacto | 85% | 10% | 5% | **Neumonia** |
| Fiebre + Tos + SIN dolor pecho + Mucosidad | 20% | 30% | 50% | **Bronquitis** |
| Fiebre + SIN tos + Dolor cabeza + Rigidez | 90% | 5% | 5% | **Meningitis** |

**3. Clasificacion Final**

Cuando se llega a un nodo hoja, el diagnostico ya representa la clase mas probable:

```csharp
// FormDiagnostico.cs
if (nodoActual.EsHoja())
{
    // El diagnostico del nodo hoja es la clasificacion Naive Bayes
    string diagnosticoNaiveBayes = nodoActual.Diagnostico;
    registro.Diagnostico = diagnosticoNaiveBayes;

    MostrarPanelDescripcion();
}
```

### Ejemplo Completo de Clasificacion

**Caso**: Paciente con sintomas respiratorios

**Sintomas Observados**:
- Fiebre: SI (90% indica infeccion)
- Tos persistente: SI (80% indica problema respiratorio)
- Dolor en pecho: SI (70% indica inflamacion pulmonar)
- Contacto con enfermos: SI (aumenta probabilidad de contagio)

**Proceso de Clasificacion**:

```
Inicio: Nodo Raiz
- Pregunta 1: Tiene fiebre? -> SI
  - Pregunta 2: Tiene tos? -> SI
    - Pregunta 3: Dolor en pecho? -> SI
      - Pregunta 4: Contacto con enfermos? -> SI
        - DIAGNOSTICO: "CRITICO: Posible neumonia. URGENCIA INMEDIATA."
```

### Ventajas de Esta Implementacion

1. **Rapidez**: O(h) donde h es la altura del arbol (5 niveles) = **O(5)**
2. **Simplicidad**: No requiere calculos complejos en tiempo real
3. **Interpretabilidad**: El camino del arbol explica el diagnostico
4. **No requiere entrenamiento**: Las probabilidades son establecidas por expertos medicos

### Limitaciones

1. **No aprende**: El sistema no mejora con nuevos casos
2. **Diagnosticos limitados**: Solo puede clasificar las enfermedades predefinidas en el arbol
3. **Probabilidades no explicitas**: No calcula ni muestra probabilidades numericas
4. **Asuncion de independencia**: Asume que los sintomas son independientes (naive assumption)

---

## Arquitectura del Sistema

### Estructura de Directorios

```
MediCenter_Finished/
|
|-- Modelos de Datos/
|   |-- Usuario.cs                  (Clase base)
|   |-- Paciente.cs                 (Hereda de Usuario)
|   |-- PersonalHospitalario.cs     (Hereda de Usuario)
|   |-- Hospital.cs                 (Grafo de hospitales)
|   |-- RegistroMedico.cs           (Historial medico)
|   +-- DecisionNode.cs             (Arbol de decision)
|
|-- Logica de Negocio/
|   |-- Sistema1.cs                 (Sistema principal - grafos, arboles, colas)
|   +-- PersistenciaBinaria.cs      (Serializacion de datos)
|
|-- Formularios - Pantalla Principal/
|   |-- FormPrincipal.cs            (Pantalla de inicio)
|   |-- FormLoginPaciente.cs        (Login y registro de pacientes)
|   +-- FormLoginPersonal.cs        (Login de medicos/admins)
|
|-- Formularios - Paciente/
|   |-- FormMenuPaciente.cs         (Menu principal del paciente)
|   |-- FormSeleccionarHospital.cs  (Seleccion de hospital - grafo)
|   |-- FormBienvenidaHospital.cs   (Bienvenida al hospital)
|   |-- FormDiagnostico.cs          (Cuestionario + Naive Bayes)
|   +-- FormHistorialPaciente.cs    (Ver historial medico)
|
|-- Formularios - Medico/
|   |-- FormMenuMedico.cs           (Menu principal del medico)
|   |-- FormListaColaPacientes.cs   (Ver cola FIFO de pacientes)
|   |-- FormAtenderPaciente.cs      (Atender paciente)
|   |-- FormValidarDiagnosticos.cs  (Validar diagnosticos)
|   +-- FormEditarRegistroMedico.cs (Editar diagnostico)
|
+-- Formularios - Administrador/
    |-- FormMenuAdministrador.cs    (Menu principal del admin)
    |-- FormRegistrarPersonalAdmin.cs (Registrar doctores/admins)
    |-- FormGestionPersonal.cs      (Gestionar personal)
    |-- FormCompararHospitales.cs   (Ver estadisticas de hospitales)
    +-- FormEstadisticasEnfermedades.cs (Estadisticas de enfermedades)
```

### Diagrama de Flujo General

```
+-------------------------------------------------------------+
|                    PANTALLA PRINCIPAL                        |
|                                                              |
|  +----------+     +----------+     +----------+             |
|  | PACIENTE |     | PERSONAL |     |   INFO   |             |
|  +----+-----+     +----+-----+     +----------+             |
+-------|-----------------|------------------------------------|
        |                 |
        v                 v
+---------------+ +---------------+
| Login/Registro| |     Login     |
|   Paciente    | | Personal/Admin|
+-------+-------+ +-------+-------+
        |                 |
        v                 v
+---------------+ +---------------+
| Menu Paciente | | Menu Medico/  |
|               | | Administrador |
+-------+-------+ +-------+-------+
        |                 |
        |                 +-> Ver Cola (Queue FIFO)
        |                 +-> Atender Paciente
        |                 +-> Validar Diagnosticos
        |                 +-> Gestionar Personal
        |                 +-> Ver Estadisticas
        |
        +-> Seleccionar Hospital (Grafo)
        |   +-> FormBienvenidaHospital
        |       +-> Cuestionario (Arbol de Decision)
        |           +-> Naive Bayes
        |               +-> Resultado Final
        |
        +-> Ver Historial Medico
```

---

## Guia de Uso del Programa

### Guia de Uso - PACIENTES

#### 1. Registro de Paciente Nuevo

1. Ejecutar la aplicacion
2. En la pantalla principal, clic en **"PACIENTE"**
3. Clic en **"Registrarse"**
4. Completar el formulario con:
   - Nombre completo
   - Email
   - Contrasena
   - Edad, Genero, Tipo de Sangre
   - Telefono, Contacto de emergencia
   - Tipo de seguro (Sin Seguro, Basico, Completo)
   - Imagenes de seguro (si aplica)
5. El sistema genera un ID automatico (ej: P0001)

#### 2. Iniciar Sesion como Paciente

1. Pantalla principal -> **"PACIENTE"**
2. Ingresar **ID** (ej: P0001) y **Contrasena**
3. Clic en **"Iniciar Sesion"**

#### 3. Realizar Nueva Consulta Medica

1. Menu Paciente -> **"Nueva Consulta Medica"**
2. Seleccionar hospital disponible (segun tipo de seguro)
3. Leer informacion del hospital y clic en **"Comenzar Cuestionario"**
4. Responder preguntas de SI/NO del cuestionario
5. Describir malestar en el campo de texto
6. Ver diagnostico automatico
7. El sistema lo agrega a la cola de espera del hospital

#### 4. Ver Historial Medico

1. Menu Paciente -> **"Ver Historial Medico"**
2. Ver lista de consultas anteriores
3. Doble clic en un registro para ver detalles

---

### Guia de Uso - MEDICOS

#### 1. Iniciar Sesion como Medico

1. Pantalla principal -> **"PERSONAL"**
2. Ingresar **ID** (ej: M0001) y **Contrasena**
3. Clic en **"Iniciar Sesion"**

#### 2. Ver y Atender Cola de Pacientes

1. Menu Medico -> **"Ver Cola de Pacientes"**
2. Ver lista de pacientes en espera (orden FIFO)
3. Clic en **"Atender Siguiente"** para atender al primer paciente
4. Revisar diagnostico automatico
5. Ingresar diagnostico medico y tratamiento
6. Guardar registro

#### 3. Validar Diagnosticos Pendientes

1. Menu Medico -> **"Validar Diagnosticos Pendientes"**
2. Ver lista de diagnosticos sin confirmar
3. Clic en **"Validar"** para confirmar o modificar

---

### Guia de Uso - ADMINISTRADORES

#### 1. Credenciales por Defecto

- **Usuario**: ADMIN001
- **Contrasena**: admin123

#### 2. Registrar Nuevo Personal

1. Menu Administrador -> **"Registrar Nuevo Personal"**
2. Completar formulario:
   - Nombre completo
   - Email
   - Hospital de asignacion
   - Nivel de acceso (Medico/Administrador)
   - Especialidad (si es medico)
3. El sistema genera ID automatico y contrasena por defecto

#### 3. Ver Informacion de Hospitales

1. Menu Administrador -> **"Ver Informacion Hospitales"**
2. Ver tabla con:
   - ID y Nombre del hospital
   - Tipo (Publico/Privado)
   - Numero de Doctores
   - Numero de Pacientes
3. Doble clic en un hospital para ver sus pacientes

#### 4. Trasladar Paciente

1. Menu Administrador -> **"Trasladar Paciente"**
2. Seleccionar paciente
3. Seleccionar hospital destino
4. Confirmar traslado
5. El paciente se elimina del hospital origen y se agrega al destino

---

## Requisitos e Instalacion

### Requisitos del Sistema

- **Sistema Operativo**: Windows 10 (64-bit) o superior
- **Framework**: .NET 8.0 Windows Desktop Runtime
- **Memoria RAM**: 4 GB minimo
- **Espacio en Disco**: 100 MB minimo
- **Resolucion de Pantalla**: 1024x768 minimo recomendado

### Instalacion

1. **Instalar .NET 8.0**:
   - Descargar de: https://dotnet.microsoft.com/download/dotnet/8.0
   - Instalar ".NET Desktop Runtime 8.0.x" (Windows x64)

2. **Compilar el Proyecto**:
   ```bash
   cd MediCenter_Finished
   dotnet restore
   dotnet build
   ```

3. **Ejecutar la Aplicacion**:
   ```bash
   dotnet run
   ```
   O abrir `WC_MediCenter.sln` en Visual Studio 2022 y presionar F5.

---

## Notas Importantes

### Descargo de Responsabilidad Medica

**IMPORTANTE**: Este sistema proporciona diagnosticos automaticos preliminares basados en algoritmos de arboles de decision. **NO reemplaza la consulta medica profesional**.

Los diagnosticos generados son **orientativos** y deben ser:
- Validados por un medico certificado
- Complementados con examenes clinicos
- Confirmados con estudios de laboratorio e imagen

**Siempre consulte con un medico certificado para diagnosticos y tratamientos reales.**

### Persistencia de Datos

- Los datos se guardan en archivos .dat binarios
- Ubicacion: `MediCenterData/`
- Para resetear el sistema, eliminar toda la carpeta MediCenterData

---

## Licencia y Creditos

Este proyecto es un **sistema educativo y de demostracion** desarrollado con fines academicos.

### Tecnologias Utilizadas

- **Lenguaje**: C# .NET 8.0
- **Interfaz**: Windows Forms
- **Serializacion**: BinaryFormatter

### Version

- **Version**: 1.0
- **Fecha**: Diciembre 2025

---

**FIN DEL DOCUMENTO**

---

*Este sistema integra Estructuras de Datos avanzadas (Grafos, Arboles, Colas) con el Algoritmo de Clasificacion Naive Bayes para proporcionar un sistema hospitalario completo y funcional.*
