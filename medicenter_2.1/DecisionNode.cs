using System.Collections.Generic;
﻿
﻿namespace MEDICENTER
﻿{
﻿    // Clase DecisionNode: representa un nodo dentro de un árbol de decisiones.
﻿    // Utilizado para construir el árbol de diagnóstico de síntomas.
﻿    public class DecisionNode
﻿    {
﻿        public string Id { get; set; } // Identificador único del nodo.
﻿        public string Pregunta { get; set; } // La pregunta asociada a este nodo (si no es un nodo hoja de diagnóstico).
﻿        public string Diagnostico { get; set; } // El diagnóstico final si este nodo es una hoja (terminal).
﻿        public List<DecisionNode> Hijos { get; set; } // Lista de nodos hijos, representando las posibles ramificaciones.
﻿        public string RespuestaEsperada { get; set; } // La respuesta del usuario que lleva a este nodo desde su nodo padre.
﻿
﻿        // Constructor para nodos de pregunta.
﻿        public DecisionNode(string id, string pregunta)
﻿        {
﻿            Id = id;
﻿            Pregunta = pregunta; // Inicializa la pregunta del nodo.
﻿            Hijos = new List<DecisionNode>(); // Inicializa la lista de hijos vacía.
﻿        }
﻿
﻿        // Constructor para nodos hoja que representan un diagnóstico final.
﻿        public DecisionNode(string id, string diagnostico, bool esHoja)
﻿        {
﻿            Id = id;
﻿            Diagnostico = diagnostico; // Establece el diagnóstico final.
﻿            Hijos = new List<DecisionNode>(); // Aunque sea una hoja, la lista de hijos se inicializa (estará vacía).
﻿            // El parámetro 'esHoja' se usa para indicar la intención, pero la lógica de EsHoja() lo determinará.
﻿        }
﻿
﻿        // Método para determinar si este nodo es un nodo hoja (terminal).
﻿        // Un nodo es hoja si tiene un diagnóstico y no tiene hijos.
﻿        public bool EsHoja()
﻿        {
﻿            return !string.IsNullOrEmpty(Diagnostico) && Hijos.Count == 0;
﻿        }
﻿
﻿        // Método para agregar un nodo hijo a la lista de hijos de este nodo.
﻿        public void AgregarHijo(DecisionNode hijo)
﻿        {
﻿            Hijos.Add(hijo);
﻿        }
﻿    }
﻿}
﻿