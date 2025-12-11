using System;
using System.Collections.Generic;
using System.Linq;

namespace MEDICENTER
{
    // Clase principal del sistema MediCenter que gestiona toda la lógica de negocio y los datos.
    // Actúa como un punto central para acceder y manipular información de pacientes, personal y hospitales.
    public class Sistema
    {
        // Propiedades públicas que almacenan colecciones de las entidades principales del sistema.
        public List<Paciente> Pacientes { get; set; } // Lista de todos los pacientes registrados.
        public List<PersonalHospitalario> Personal { get; set; } // Lista de todo el personal hospitalario (médicos, administradores).
        public List<Hospital> Hospitales { get; set; } // Lista de todos los hospitales disponibles en el sistema.
        public DecisionNode ArbolDiagnostico { get; set; } // Raíz del árbol de decisiones para diagnósticos.
        public Dictionary<string, Queue<string>> ColasPorHospital { get; set; } // Diccionario para gestionar colas de pacientes por hospital.
        public Dictionary<string, List<RegistroMedico>> RegistrosPorHospital { get; set; } // Diccionario para almacenar registros médicos por hospital.

        // Contadores internos para generar IDs únicos para pacientes, personal y registros.
        private int contadorPacientes;
        private int contadorPersonal;
        private int contadorRegistros;
        
        // Instancia del manejador de persistencia binaria para guardar y cargar datos.
        internal readonly PersistenciaBinaria _persistencia; // Hacemos internal para acceso directo para eliminación
        
        // Constructor de la clase Sistema.
        public Sistema()
        {
            _persistencia = new PersistenciaBinaria(); // Inicializa el objeto de persistencia.

            // Asegura que las carpetas de datos existan y carga todos los registros existentes.
            _persistencia.AsegurarCarpetasDeDatos();
            var usuariosCargados = _persistencia.CargarTodosLosRegistros();
            
            // Separa los usuarios cargados en listas de Pacientes y PersonalHospitalario.
            Pacientes = usuariosCargados.OfType<Paciente>().ToList();
            Personal = usuariosCargados.OfType<PersonalHospitalario>().ToList();

            // Inicialización de colecciones para datos no persistentes de forma global.
            Hospitales = new List<Hospital>();
            ColasPorHospital = new Dictionary<string, Queue<string>>();
            RegistrosPorHospital = new Dictionary<string, List<RegistroMedico>>();

            // Inicialización de contadores basado en los datos cargados para asegurar IDs únicos.
            if (Pacientes.Any())
            {
                // Si hay pacientes, el contador empieza desde el ID más alto existente + 1.
                contadorPacientes = Pacientes.Max(p => int.Parse(p.Id.Substring(1))) + 1;
            }
            else
            {
                // Si no hay pacientes, el contador empieza en 1.
                contadorPacientes = 1;
            }
            
            if (Personal.Any(p => p.Id.StartsWith("M")))
            {
                // Si hay personal con ID que comienza con 'M', el contador empieza desde el ID más alto existente + 1.
                contadorPersonal = Personal.Where(p => p.Id.StartsWith("M"))
                                           .Max(p => int.Parse(p.Id.Substring(1))) + 1;
            }
            else
            {
                // Si no hay personal con ID 'M', el contador empieza en 1.
                contadorPersonal = 1; 
            }
            
            contadorRegistros = 1; // Este se podría calcular si fuera necesario

            // Configuración de datos iniciales del sistema que no se persisten automáticamente.
            InicializarHospitales(); // Carga hospitales predefinidos.
            InicializarArbolDiagnostico(); // Configura el árbol de decisiones para diagnósticos.
            
            // Crea un administrador por defecto si no existe y lo guarda.
            InicializarAdministradorPorDefecto();
        }
        
        // Guarda un usuario (Paciente o PersonalHospitalario) usando el sistema de persistencia.
        public void GuardarUsuario(Usuario usuario)
        {
            _persistencia.GuardarRegistro(usuario);
        }
        
        // Elimina un usuario del sistema (paciente o personal) y de la persistencia.
        public void EliminarUsuario(string idUsuario)
        {
            // Intenta eliminar de la lista de Pacientes.
            Paciente pacienteAEliminar = Pacientes.FirstOrDefault(p => p.Id == idUsuario);
            if (pacienteAEliminar != null)
            {
                Pacientes.Remove(pacienteAEliminar);
            }
            else
            {
                // Si no es un paciente, intenta eliminar de la lista de Personal.
                PersonalHospitalario personalAEliminar = Personal.FirstOrDefault(p => p.Id == idUsuario);
                if (personalAEliminar != null)
                {
                    Personal.Remove(personalAEliminar);
                    // Si el personal estaba asignado a un hospital, también se elimina de la lista de IDs de personal de ese hospital.
                    Hospital hospitalAsignado = Hospitales.FirstOrDefault(h => h.Id == personalAEliminar.IdHospital);
                    if (hospitalAsignado != null && hospitalAsignado.PersonalIds.Contains(personalAEliminar.Id))
                    {
                        hospitalAsignado.PersonalIds.Remove(personalAEliminar.Id);
                    }
                }
            }
            // Elimina el registro del usuario de la persistencia.
            _persistencia.EliminarRegistro(idUsuario);
        }
        
        // Inicializa la lista de hospitales con datos predefinidos.
        private void InicializarHospitales()
        {
            Hospitales = new List<Hospital>
            {
                new Hospital { Id = "H001", Nombre = "Hospital Manolo Morales Peralta", EsPublico = true, CostoConsulta = 0, PrecisionDiagnostico = 85, TiempoPromedioMin = 45 },
                new Hospital { Id = "H002", Nombre = "Hospital Velez Paiz", EsPublico = true, CostoConsulta = 0, PrecisionDiagnostico = 82, TiempoPromedioMin = 50 },
                new Hospital { Id = "H003", Nombre = "Hospital Bautista", EsPublico = false, CostoConsulta = 200.00m, PrecisionDiagnostico = 95, TiempoPromedioMin = 25 },
                new Hospital { Id = "H004", Nombre = "Hospital Vivian Pellas", EsPublico = false, CostoConsulta = 220.00m, PrecisionDiagnostico = 97, TiempoPromedioMin = 20 }
            };

            // Para cada hospital, inicializa su cola de pacientes y su lista de registros médicos.
            foreach (var hospital in Hospitales)
            {
                ColasPorHospital[hospital.Id] = new Queue<string>(); // Cola de IDs de pacientes esperando.
                RegistrosPorHospital[hospital.Id] = new List<RegistroMedico>(); // Lista de registros médicos del hospital.
            }
        }

        // Inicializa un usuario administrador por defecto si no existe en el sistema.
        private void InicializarAdministradorPorDefecto()
        {
            // Verifica si ya existe un administrador con el ID "ADMIN001".
            if (Personal.Exists(p => p.Id == "ADMIN001"))
                return; // Si existe, no hace nada.

            // Crea un nuevo objeto PersonalHospitalario para el administrador por defecto.
            PersonalHospitalario adminDefault = new PersonalHospitalario
            {
                Id = "ADMIN001",
                Nombre = "Administrador General",
                Email = "admin@medicenter.com",
                Password = "admin123",
                IdHospital = "H001", // Asigna al primer hospital por defecto.
                NivelAcceso = NivelAcceso.Administrador, // Define su nivel de acceso.
                CambioPassword = true // Indica que debe cambiar su contraseña en el primer login.
            };

            Personal.Add(adminDefault); // Añade el administrador a la lista de personal.
            GuardarUsuario(adminDefault); // Guarda el administrador en la persistencia.
        }

        // Inicializa el árbol de decisiones mejorado para el diagnóstico de síntomas.
        // Árbol expandido con más preguntas y enfermedades especulativas.
        private void InicializarArbolDiagnostico()
        {
            // Nivel 1: Nodo raíz con la primera pregunta.
            ArbolDiagnostico = new DecisionNode("root", "¿Tiene fiebre o temperatura elevada (más de 38°C)?");

            // --- RAMA: Tiene fiebre (SI) ---
            DecisionNode nodoFiebreSi = new DecisionNode("fiebre_si", "¿Tiene tos persistente y dificultad para respirar?");
            nodoFiebreSi.RespuestaEsperada = "si";

            // SI: Tos + Dificultad respiratoria
            DecisionNode nodoTosDificultadSi = new DecisionNode("tos_dificultad_si", "¿Presenta dolor en el pecho al respirar?");
            nodoTosDificultadSi.RespuestaEsperada = "si";

            // SI: Dolor en pecho
            DecisionNode nodoDolorPechoSi = new DecisionNode("dolor_pecho_si", "¿La respiración es muy rápida (más de 25 respiraciones por minuto) o tiene los labios/dedos azulados?");
            nodoDolorPechoSi.RespuestaEsperada = "si";

            // Pregunta adicional sobre saturación de oxígeno
            DecisionNode nodoSaturacionBaja = new DecisionNode("saturacion_baja", "¿Ha tenido contacto con personas enfermas o viajado recientemente?");
            nodoSaturacionBaja.RespuestaEsperada = "si";

            DecisionNode diagNeumoniaCovid = new DecisionNode("diag_neumonia_covid", "CRÍTICO: Posible neumonía bacteriana/viral severa, COVID-19, SDRA (Síndrome de Distrés Respiratorio Agudo), o embolia pulmonar. Signos de hipoxemia (bajo oxígeno). URGENCIA MÉDICA INMEDIATA - LLAME AMBULANCIA.", true);
            diagNeumoniaCovid.RespuestaEsperada = "si";

            DecisionNode diagNeumoniaAspirac = new DecisionNode("diag_neumonia_aspirac", "CRÍTICO: Posible neumonía atípica, bronquitis aguda severa, insuficiencia respiratoria o edema pulmonar. Signos de dificultad respiratoria grave. Consulte médico URGENTEMENTE.", true);
            diagNeumoniaAspirac.RespuestaEsperada = "no";

            nodoSaturacionBaja.AgregarHijo(diagNeumoniaCovid);
            nodoSaturacionBaja.AgregarHijo(diagNeumoniaAspirac);

            // Sin signos de hipoxemia severa
            DecisionNode nodoSaturacionNormal = new DecisionNode("saturacion_normal", "¿Tiene expectoración con sangre o sangrado al toser?");
            nodoSaturacionNormal.RespuestaEsperada = "no";

            DecisionNode diagHemoptisis = new DecisionNode("diag_hemoptisis", "CRÍTICO: Hemoptisis (sangre en esputo). Posible tuberculosis, cáncer pulmonar, bronquiectasias o tromboembolismo pulmonar. Requiere evaluación inmediata.", true);
            diagHemoptisis.RespuestaEsperada = "si";

            DecisionNode diagBronquitisAguda = new DecisionNode("diag_bronquitis_aguda", "MODERADO-GRAVE: Posible bronquitis aguda, neumonía leve o infección respiratoria complicada. Monitoreo médico necesario. Consulte si empeora.", true);
            diagBronquitisAguda.RespuestaEsperada = "no";

            nodoSaturacionNormal.AgregarHijo(diagHemoptisis);
            nodoSaturacionNormal.AgregarHijo(diagBronquitisAguda);

            nodoDolorPechoSi.AgregarHijo(nodoSaturacionBaja);
            nodoDolorPechoSi.AgregarHijo(nodoSaturacionNormal);

            // NO: Sin dolor en pecho pero con tos
            DecisionNode nodoDolorPechoNo = new DecisionNode("dolor_pecho_no", "¿La tos es seca e irritativa, o produce mucosidad/flema?");
            nodoDolorPechoNo.RespuestaEsperada = "no";

            // Tos productiva (con flema)
            DecisionNode nodoTosProductiva = new DecisionNode("tos_productiva", "¿La mucosidad es verde, amarilla o de mal olor?");
            nodoTosProductiva.RespuestaEsperada = "si";

            DecisionNode diagBronquitisBacteriana = new DecisionNode("diag_bronquitis_bact", "MODERADO: Posible bronquitis bacteriana o infección respiratoria aguda con sobreinfección. La coloración del esputo indica probable infección bacteriana. Antibióticos pueden ser necesarios. Consulte médico.", true);
            diagBronquitisBacteriana.RespuestaEsperada = "si";

            DecisionNode diagBronquitisViral = new DecisionNode("diag_bronquitis_viral", "LEVE-MODERADO: Posible bronquitis viral o traqueobronquitis. Esputo claro/blanquecino indica proceso viral. Reposo, líquidos, expectorantes. Consulte si persiste más de 7 días.", true);
            diagBronquitisViral.RespuestaEsperada = "no";

            nodoTosProductiva.AgregarHijo(diagBronquitisBacteriana);
            nodoTosProductiva.AgregarHijo(diagBronquitisViral);

            // Tos seca
            DecisionNode nodoTosSeca = new DecisionNode("tos_seca", "¿Presenta pérdida del olfato o gusto, o dolor de cuerpo intenso?");
            nodoTosSeca.RespuestaEsperada = "no";

            DecisionNode diagCovid19Like = new DecisionNode("diag_covid_like", "MODERADO: Posible COVID-19, influenza o infección viral respiratoria con síntomas sistémicos. Aislamiento preventivo recomendado. Monitoreo de saturación de oxígeno. Consulte si empeora.", true);
            diagCovid19Like.RespuestaEsperada = "si";

            DecisionNode diagGripeComun = new DecisionNode("diag_gripe_comun", "LEVE-MODERADO: Posible gripe común, resfriado o rinofaringitis viral. Tos seca irritativa. Reposo, líquidos abundantes, antipiréticos. Monitoreo constante.", true);
            diagGripeComun.RespuestaEsperada = "no";

            nodoTosSeca.AgregarHijo(diagCovid19Like);
            nodoTosSeca.AgregarHijo(diagGripeComun);

            nodoDolorPechoNo.AgregarHijo(nodoTosProductiva);
            nodoDolorPechoNo.AgregarHijo(nodoTosSeca);

            nodoTosDificultadSi.AgregarHijo(nodoDolorPechoSi);
            nodoTosDificultadSi.AgregarHijo(nodoDolorPechoNo);

            // NO: Sin tos ni dificultad respiratoria
            DecisionNode nodoTosDificultadNo = new DecisionNode("tos_dificultad_no", "¿Tiene dolor de cabeza intenso o rigidez en el cuello?");
            nodoTosDificultadNo.RespuestaEsperada = "no";

            // SI: Dolor cabeza intenso/rigidez cuello
            DecisionNode nodoCabezaCuelloSi = new DecisionNode("cabeza_cuello_si", "¿Presenta sensibilidad extrema a la luz (fotofobia) o vómitos en proyectil?");
            nodoCabezaCuelloSi.RespuestaEsperada = "si";

            // Preguntas adicionales sobre meningitis
            DecisionNode nodoSignosMeningitis = new DecisionNode("signos_meningitis", "¿Tiene confusión mental, somnolencia excesiva, manchas rojas en la piel o convulsiones?");
            nodoSignosMeningitis.RespuestaEsperada = "si";

            DecisionNode diagMeningitisGrave = new DecisionNode("diag_meningitis_grave", "CRÍTICO: Alta probabilidad de meningitis bacteriana o encefalitis. Signos de compromiso neurológico. EMERGENCIA ABSOLUTA - LLAME AMBULANCIA INMEDIATAMENTE. No espere, cada minuto cuenta.", true);
            diagMeningitisGrave.RespuestaEsperada = "si";

            DecisionNode diagMeningitis = new DecisionNode("diag_meningitis", "CRÍTICO: Posible meningitis viral/bacteriana o infección del sistema nervioso central. Tríada clásica: cefalea + rigidez nucal + fotofobia. URGENCIA - ACUDA A EMERGENCIAS INMEDIATAMENTE.", true);
            diagMeningitis.RespuestaEsperada = "no";

            nodoSignosMeningitis.AgregarHijo(diagMeningitisGrave);
            nodoSignosMeningitis.AgregarHijo(diagMeningitis);

            // Sin signos meníngeos completos
            DecisionNode nodoSinMeningitis = new DecisionNode("sin_meningitis", "¿El dolor es pulsátil (late) y empeora con luz, ruido o movimiento?");
            nodoSinMeningitis.RespuestaEsperada = "no";

            DecisionNode diagMigranaSevera = new DecisionNode("diag_migrana_severa", "MODERADO: Posible migraña aguda con fiebre o cefalea tensional severa. Aunque la rigidez es preocupante, los síntomas sugieren migraña. Analgésicos fuertes, reposo en oscuridad. Consulte si no mejora en 24h.", true);
            diagMigranaSevera.RespuestaEsperada = "si";

            DecisionNode diagSinusitis = new DecisionNode("diag_sinusitis", "MODERADO: Posible sinusitis aguda, cefalea de origen viral o tensional con fiebre. Rigidez cervical leve puede ser muscular. Descongestionantes, analgésicos. Consulte si empeora.", true);
            diagSinusitis.RespuestaEsperada = "no";

            nodoSinMeningitis.AgregarHijo(diagMigranaSevera);
            nodoSinMeningitis.AgregarHijo(diagSinusitis);

            nodoCabezaCuelloSi.AgregarHijo(nodoSignosMeningitis);
            nodoCabezaCuelloSi.AgregarHijo(nodoSinMeningitis);

            // NO: Sin dolor cabeza intenso
            DecisionNode nodoCabezaCuelloNo = new DecisionNode("cabeza_cuello_no", "¿Presenta dolor muscular y articular generalizado?");
            nodoCabezaCuelloNo.RespuestaEsperada = "no";

            DecisionNode diagDengueChik = new DecisionNode("diag_dengue_chik", "MODERADO-CRÍTICO: Posible dengue, chikungunya, fiebre viral hemorrágica o infección arboviral. Consulte médico urgente. Hidratación crucial.", true);
            diagDengueChik.RespuestaEsperada = "si";

            DecisionNode diagInfeccionViral = new DecisionNode("diag_infeccion_viral", "LEVE-MODERADO: Posible infección viral sistémica, gripe estacional o síndrome viral inespecífico. Reposo, hidratación, antipiréticos.", true);
            diagInfeccionViral.RespuestaEsperada = "no";

            nodoCabezaCuelloNo.AgregarHijo(diagDengueChik);
            nodoCabezaCuelloNo.AgregarHijo(diagInfeccionViral);

            nodoTosDificultadNo.AgregarHijo(nodoCabezaCuelloSi);
            nodoTosDificultadNo.AgregarHijo(nodoCabezaCuelloNo);

            nodoFiebreSi.AgregarHijo(nodoTosDificultadSi);
            nodoFiebreSi.AgregarHijo(nodoTosDificultadNo);


            // --- RAMA: No tiene fiebre (NO) ---
            DecisionNode nodoFiebreNo = new DecisionNode("fiebre_no", "¿Tiene náuseas, vómitos o dolor abdominal intenso?");
            nodoFiebreNo.RespuestaEsperada = "no";

            // SI: Náuseas/vómitos/dolor abdominal
            DecisionNode nodoNauseasVomitosSi = new DecisionNode("nauseas_vomitos_si", "¿El dolor abdominal es agudo y localizado en el lado derecho inferior?");
            nodoNauseasVomitosSi.RespuestaEsperada = "si";

            // SI: Dolor localizado derecho inferior
            DecisionNode nodoDolorDerechoSi = new DecisionNode("dolor_derecho_si", "¿El dolor aumenta al presionar y luego soltar (signo de rebote/Blumberg)?");
            nodoDolorDerechoSi.RespuestaEsperada = "si";

            // Pregunta adicional sobre signos de peritonitis
            DecisionNode nodoSignosApendicitis = new DecisionNode("signos_apendicitis", "¿Presenta fiebre superior a 38°C, náuseas intensas o imposibilidad de caminar erguido por el dolor?");
            nodoSignosApendicitis.RespuestaEsperada = "si";

            DecisionNode diagApendicitis = new DecisionNode("diag_apendicitis", "CRÍTICO: Alta probabilidad de apendicitis aguda con posible peritonitis. Signo de Blumberg positivo + fiebre + dolor migratorio. URGENCIA QUIRÚRGICA INMEDIATA - ACUDA A EMERGENCIAS. NO COMA NI BEBA NADA.", true);
            diagApendicitis.RespuestaEsperada = "si";

            DecisionNode diagApendicitisInicial = new DecisionNode("diag_apendicitis_inicial", "CRÍTICO: Posible apendicitis aguda en fase inicial. Signos de irritación peritoneal. Requiere evaluación quirúrgica urgente. ACUDA A EMERGENCIAS.", true);
            diagApendicitisInicial.RespuestaEsperada = "no";

            nodoSignosApendicitis.AgregarHijo(diagApendicitis);
            nodoSignosApendicitis.AgregarHijo(diagApendicitisInicial);

            // Sin signo de rebote
            DecisionNode nodoSinRebote = new DecisionNode("sin_rebote", "¿El dolor comenzó alrededor del ombligo y luego se movió al lado derecho?");
            nodoSinRebote.RespuestaEsperada = "no";

            DecisionNode diagApendicitisTipica = new DecisionNode("diag_apendicitis_tipica", "MODERADO-GRAVE: Posible apendicitis con presentación típica (dolor migratorio). Aunque no hay rebote claro, requiere evaluación médica urgente. Consulte emergencias.", true);
            diagApendicitisTipica.RespuestaEsperada = "si";

            DecisionNode diagColitis = new DecisionNode("diag_colitis", "MODERADO: Posible colitis derecha, enfermedad inflamatoria intestinal (Crohn, colitis ulcerosa) o ileítis terminal. Consulte gastroenterólogo.", true);
            diagColitis.RespuestaEsperada = "no";

            nodoSinRebote.AgregarHijo(diagApendicitisTipica);
            nodoSinRebote.AgregarHijo(diagColitis);

            nodoDolorDerechoSi.AgregarHijo(nodoSignosApendicitis);
            nodoDolorDerechoSi.AgregarHijo(nodoSinRebote);

            // NO: Dolor no localizado o difuso
            DecisionNode nodoDolorDerechoNo = new DecisionNode("dolor_derecho_no", "¿Presenta diarrea líquida abundante con más de 5 evacuaciones al día?");
            nodoDolorDerechoNo.RespuestaEsperada = "no";

            DecisionNode diagGastroenteritisAguda = new DecisionNode("diag_gastroenteritis_aguda", "MODERADO: Posible gastroenteritis aguda, intoxicación alimentaria o infección gastrointestinal. Hidratación oral crucial. Consulte si empeora.", true);
            diagGastroenteritisAguda.RespuestaEsperada = "si";

            DecisionNode diagGastritis = new DecisionNode("diag_gastritis", "LEVE-MODERADO: Posible gastritis, dispepsia o síndrome de intestino irritable. Dieta blanda, evite irritantes. Antiácidos.", true);
            diagGastritis.RespuestaEsperada = "no";

            nodoDolorDerechoNo.AgregarHijo(diagGastroenteritisAguda);
            nodoDolorDerechoNo.AgregarHijo(diagGastritis);

            nodoNauseasVomitosSi.AgregarHijo(nodoDolorDerechoSi);
            nodoNauseasVomitosSi.AgregarHijo(nodoDolorDerechoNo);

            // NO: Sin náuseas/vómitos/dolor abdominal
            DecisionNode nodoNauseasVomitosNo = new DecisionNode("nauseas_vomitos_no", "¿Experimenta dolor de garganta o dificultad para tragar?");
            nodoNauseasVomitosNo.RespuestaEsperada = "no";

            // SI: Dolor de garganta
            DecisionNode nodoDolorGargantaSi = new DecisionNode("dolor_garganta_si", "¿Observa placas blancas o pus en las amígdalas?");
            nodoDolorGargantaSi.RespuestaEsperada = "si";

            DecisionNode diagAmigdalitisBact = new DecisionNode("diag_amigdalitis_bact", "MODERADO: Posible amigdalitis bacteriana (estreptococo), faringitis purulenta. Antibióticos necesarios. Consulte médico.", true);
            diagAmigdalitisBact.RespuestaEsperada = "si";

            DecisionNode diagFaringitisViral = new DecisionNode("diag_faringitis_viral", "LEVE: Posible faringitis viral, laringitis o irritación de garganta. Gárgaras, analgésicos, líquidos calientes.", true);
            diagFaringitisViral.RespuestaEsperada = "no";

            nodoDolorGargantaSi.AgregarHijo(diagAmigdalitisBact);
            nodoDolorGargantaSi.AgregarHijo(diagFaringitisViral);

            // NO: Sin dolor de garganta
            DecisionNode nodoDolorGargantaNo = new DecisionNode("dolor_garganta_no", "¿Tiene dolor de espalda baja o dificultad/dolor al orinar?");
            nodoDolorGargantaNo.RespuestaEsperada = "no";

            // SI: Dolor espalda/problemas urinarios
            DecisionNode nodoDolorEspaldaSi = new DecisionNode("dolor_espalda_si", "¿La orina tiene mal olor, color turbio o presencia de sangre?");
            nodoDolorEspaldaSi.RespuestaEsperada = "si";

            DecisionNode diagInfeccionUrinaria = new DecisionNode("diag_infeccion_urinaria", "MODERADO: Posible infección urinaria (cistitis/pielonefritis), cálculos renales o uretritis. Antibióticos, hidratación. Consulte médico.", true);
            diagInfeccionUrinaria.RespuestaEsperada = "si";

            DecisionNode diagLumbalgia = new DecisionNode("diag_lumbalgia", "LEVE: Posible lumbalgia, contractura muscular o sobreesfuerzo físico. Analgésicos, reposo relativo, aplicar calor local.", true);
            diagLumbalgia.RespuestaEsperada = "no";

            nodoDolorEspaldaSi.AgregarHijo(diagInfeccionUrinaria);
            nodoDolorEspaldaSi.AgregarHijo(diagLumbalgia);

            // NO: Sin problemas urinarios ni espalda
            DecisionNode nodoDolorEspaldaNo = new DecisionNode("dolor_espalda_no", "¿Presenta dolor en articulaciones, hinchazón o dificultad para mover alguna extremidad?");
            nodoDolorEspaldaNo.RespuestaEsperada = "no";

            DecisionNode diagArtritis = new DecisionNode("diag_artritis", "LEVE-MODERADO: Posible artritis, bursitis, tendinitis o lesión articular. Antiinflamatorios, reposo. Consulte traumatólogo si persiste.", true);
            diagArtritis.RespuestaEsperada = "si";

            DecisionNode diagSintomasInespecificos = new DecisionNode("diag_sintomas_inespecif", "LEVE: Síntomas inespecíficos o malestar general leve. Monitoreo general. Consulte si aparecen síntomas nuevos o empeoran.", true);
            diagSintomasInespecificos.RespuestaEsperada = "no";

            nodoDolorEspaldaNo.AgregarHijo(diagArtritis);
            nodoDolorEspaldaNo.AgregarHijo(diagSintomasInespecificos);

            nodoDolorGargantaNo.AgregarHijo(nodoDolorEspaldaSi);
            nodoDolorGargantaNo.AgregarHijo(nodoDolorEspaldaNo);

            nodoNauseasVomitosNo.AgregarHijo(nodoDolorGargantaSi);
            nodoNauseasVomitosNo.AgregarHijo(nodoDolorGargantaNo);

            nodoFiebreNo.AgregarHijo(nodoNauseasVomitosSi);
            nodoFiebreNo.AgregarHijo(nodoNauseasVomitosNo);

            ArbolDiagnostico.AgregarHijo(nodoFiebreSi);
            ArbolDiagnostico.AgregarHijo(nodoFiebreNo);
        }

        // Busca un paciente en la lista de pacientes por ID y contraseña.
        public Paciente BuscarPaciente(string id, string password)
        {
            return Pacientes.Find(p => p.Id == id && p.Password == password);
        }

        // Busca personal hospitalario en la lista de personal por ID y contraseña.
        public PersonalHospitalario BuscarPersonal(string id, string password)
        {
            return Personal.Find(p => p.Id == id && p.Password == password);
        }

        // Busca un hospital en la lista de hospitales por ID.
        public Hospital BuscarHospital(string id)
        {
            return Hospitales.Find(h => h.Id == id);
        }

        // Obtiene una lista de hospitales disponibles según el tipo de seguro del paciente.
        public List<Hospital> ObtenerHospitalesDisponibles(TipoSeguro seguro)
        {
            List<Hospital> disponibles = new List<Hospital>();

            switch (seguro)
            {
                case TipoSeguro.SeguroCompleto:
                    // Con seguro completo, todos los hospitales están disponibles.
                    return new List<Hospital>(Hospitales);
                case TipoSeguro.SeguroBasico:
                case TipoSeguro.SinSeguro:
                    // Con seguro básico o sin seguro, solo los hospitales públicos están disponibles.
                    disponibles = Hospitales.FindAll(h => h.EsPublico);
                    break;
            }

            return disponibles;
        }

        // Obtiene una lista de hospitales privados.
        public List<Hospital> ObtenerHospitalesPrivados()
        {
            return Hospitales.FindAll(h => !h.EsPublico);
        }

        // Genera un nuevo ID único para un paciente.
        public string GenerarIdPaciente()
        {
            string id = $"P{contadorPacientes:D4}"; // Formato P0001, P0002, etc.
            contadorPacientes++; // Incrementa el contador para el próximo ID.
            return id;
        }

        // Genera un nuevo ID único para personal hospitalario.
        public string GenerarIdPersonal()
        {
            string id = $"M{contadorPersonal:D4}"; // Formato M0001, M0002, etc.
            contadorPersonal++; // Incrementa el contador para el próximo ID.
            return id;
        }

        // Genera un nuevo ID único para un registro médico.
        public string GenerarIdRegistro()
        {
            string id = $"R{contadorRegistros:D5}"; // Formato R00001, R00002, etc.
            contadorRegistros++; // Incrementa el contador para el próximo ID.
            return id;
        }
    }
}
﻿