# Minimal RAG API

API minimalista en ASP.NET Core (.NET 10) que implementa un flujo **RAG (Retrieval-Augmented Generation)** con fines educativos. Todos los componentes son simulados (mock) para poder ejecutarse sin dependencias externas.

---

## Requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

---

## Cómo ejecutar

```bash
# Restaurar dependencias
dotnet restore

# Ejecutar la API
dotnet run
```

La API arranca en `https://localhost:5001` (por defecto) y expone Swagger en `/swagger`.

---

## Endpoint

### `POST /api/rag/query`

Envía una pregunta y obtiene una respuesta generada con contexto recuperado.

**Request body:**

```json
{
  "question": "¿Qué son las Minimal APIs?"
}
```

**Response:**

```json
{
  "answer": "Synthesized Answer:\n\nContext used:\n- Minimal APIs are a lightweight way...\n\nQuestion: ¿Qué son las Minimal APIs?\n\nAnswer: ...",
  "retrievedChunks": [
    "Minimal APIs are a lightweight way to build HTTP APIs in ASP.NET Core.",
    "..."
  ],
  "scores": [ 0.85, 0.72, 0.61 ]
}
```

---

## Estructura del proyecto

```
Minimal_RAG_API/
  Program.cs              — Punto de entrada y definici�n del endpoint
  Models/
    RagQueryRequest.cs     — Modelo de entrada (pregunta)
    RagQueryResponse.cs    — Modelo de salida (respuesta + fragmentos + puntuaciones)
  Services/
    IEmbeddingService.cs   — Contrato para convertir texto a vector
    EmbeddingService.cs    — Implementaci�n mock de embeddings
    IVectorStore.cs        — Contrato para almacenar y buscar vectores
    VectorStore.cs         — Implementaci�n mock con b�squeda por coseno
    ILlmService.cs         — Contrato para generar respuesta
    LlmService.cs          — Implementaci�n mock de LLM
    IRagService.cs         — Contrato del orquestador RAG
    RagService.cs          — Orquestador del flujo completo
```

---

## Explicación del flujo RAG

```
  Pregunta
     |
     v
  [1] EmbeddingService  -->  vector (float[])
     |
     v
  [2] VectorStore       -->  top-3 fragmentos similares
     |
     v
  [3] LlmService        -->  respuesta sintetizada
     |
     v
  [4] RagQueryResponse  -->  JSON de salida
```

### 1. EmbeddingService (`EmbeddingService.cs`)

Convierte un texto en un vector numérico (embedding). La implementación real usaría un modelo como `text-embedding-ada-002` de OpenAI o `all-MiniLM-L6-v2` de Hugging Face. Aquí se usa un **mock deterministico**:

- Toma los bytes UTF-8 del texto
- Los distribuye en un vector de 32 dimensiones
- Normaliza el vector (magnitud = 1) para que funcione con similitud coseno

### 2. VectorStore (`VectorStore.cs`)

Almacena documentos precargados (5 fragmentos sobre .NET, RAG, etc.) y busca los más relevantes:

- Cada fragmento se embeddea al cargarse
- Al llegar una consulta, se calcula la **similitud coseno** entre el vector de la query y cada fragmento
- Devuelve los top-k fragmentos con mayor puntuación

**Similitud coseno:**

```
cos(a, b) = (a · b) / (||a|| * ||b||)
```

Un valor de 1 significa direcciones idénticas; 0 significa vectores ortogonales.

### 3. LlmService (`LlmService.cs`)

Simula un modelo de lenguaje. En producción usarías OpenAI, Anthropic, o un modelo local. Aquí:

- Muestra los fragmentos recuperados como contexto
- Identifica las **3 palabras clave** más frecuentes del contexto
- Genera una respuesta genérica basada en esas palabras clave

### 4. RagService (`RagService.cs`)

Orquestador que conecta los 3 servicios anteriores:

1. **Embed** → convierte la pregunta en vector
2. **Retrieve** → busca fragmentos relevantes en el vector store
3. **Generate** → pasa pregunta + fragmentos al LLM para obtener respuesta
4. **Return** → estructura la respuesta en `RagQueryResponse`

### 5. Program.cs

Punto de entrada. Usa **Minimal APIs** de ASP.NET Core:

- Registra los servicios en DI con `AddScoped`
- Mapea el endpoint `POST /api/rag/query`
- Valida que la pregunta no esté vacía
- Configura Swagger/OpenAPI para exploración

---

## Posibles extensiones

| Componente | Producción real |
|---|---|
| Embedding | OpenAI, Azure AI, SentenceTransformers |
| Vector Store | PostgreSQL + pgvector, Qdrant, Pinecone |
| LLM | OpenAI GPT, Anthropic Claude, Ollama (local) |
| Chunking | Fragmentación semántica real de documentos largos |
| Documentos | Carga desde PDF, web scraping, bases de datos |
