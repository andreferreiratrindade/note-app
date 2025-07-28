
# 🚀 Azure Serverless Processing with Storage Queue & Cosmos DB

## 📖 Overview

This project demonstrates a **serverless, decoupled, and scalable architecture** using Azure services. It consists of:

- An **API Gateway** that receives HTTP requests
- An **Azure Function (Producer)** that enqueues messages into an **Azure Storage Queue**
- A second **Azure Function (Consumer)** that processes the queued messages
- A **Cosmos DB** instance where the processed results are stored

This design enables asynchronous request handling and promotes scalability, cost-efficiency, and high availability.

---

## 🧱 Architecture Diagram

<img src="https://raw.githubusercontent.com/andreferreiratrindade/note-app/refs/heads/main/documents/diagram.png">


---

## 🔧 Components

| Component               | Description                                                                 |
|-------------------------|-----------------------------------------------------------------------------|
| **API Gateway**         | Receives external HTTP requests and routes them to the `enqueue` function   |
| **Function: Enqueue**   | Lightweight function that sends the request payload to the Storage Queue    |
| **Storage Queue**       | Durable queue for decoupling ingestion from processing                      |
| **Function: Process**   | Reads from the queue, performs business logic, and stores data in Cosmos DB |
| **Cosmos DB**           | NoSQL database used to persist the processed data                           |

---

## 🔥 Problems with a Purely Synchronous Architecture

In a **synchronous model**, the client sends a request and waits for the server to process everything before receiving a response. This might work well for small systems or low traffic, but it introduces serious challenges as load increases:

### ❌ 1. **Scalability Bottleneck**
- Each request ties up system resources (CPU, memory, threads) **until** the entire process finishes.
- If your process includes **slow operations** (e.g., complex business logic, DB writes, 3rd party API calls), it blocks the thread longer.
- Under high load, the app may **crash or become unresponsive**.

### ❌ 2. **User-Facing Latency**
- The user waits for the **entire chain** of operations to complete before getting a response.
- This increases **perceived latency**, hurting user experience.

### ❌ 3. **Reduced Resilience**
- A failure in any part of the flow causes the **entire request to fail**.
- No natural retry or buffering — every failure impacts the user directly.

### ❌ 4. **Poor Load Spikiness Handling**
- No built-in mechanism to **absorb or delay processing** during spikes.
- Risk of overload and crash without expensive overprovisioning.

---

## ✅ What This Architecture Solves

### ✅ 1. **Asynchronous Request Handling**
- The API accepts the request and **quickly responds**, offloading the work to background processing.

### ✅ 2. **Built-In Buffering via Queue**
- Azure Storage Queue acts as a **shock absorber**, smoothing spikes in traffic.
- No data loss or crash — just delayed processing.

### ✅ 3. **Elastic, Event-Driven Processing**
- Azure Functions **auto-scale** to match processing demand.

### ✅ 4. **Improved Reliability and Fault Tolerance**
- Message retry on failure; durable queues.
- Dead-letter queues for unprocessable messages.

### ✅ 5. **Decoupled Architecture = Maintainability**
- Each component has a **single responsibility**.
- Easier to evolve independently.

### ✅ 6. **Cost-Effective**
- **Consumption-based pricing** — you only pay when things are used.

---

## 🧠 Summary

| Scenario                        | Synchronous Approach                         | Async Architecture                             |
|--------------------------------|----------------------------------------------|------------------------------------------------|
| High request volume            | Risk of overload, timeouts                   | Queue absorbs load, functions scale            |
| Long processing time           | High latency for user                        | Fast response, async processing                |
| System failure during process  | User gets error                              | Message remains in queue, retry enabled        |
| Resource usage                 | Blocks threads and memory                    | Lightweight and event-driven                   |
| Cost                           | Needs overprovisioning for peak              | Pay-per-use, auto-scale                        |

---
