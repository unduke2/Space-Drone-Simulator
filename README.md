# DOTS vs GameObject Drone Simulation

A side-by-side benchmark of Unity’s traditional GameObject architecture versus the Entity Component System (ECS) via DOTS.

Simulates thousands of drones flying in 3D space — with dynamic movement, turret-based targeting, and projectile-based destruction — to visually demonstrate the **performance and scalability benefits** of Unity DOTS.

---

## **Project Overview**
- Compare scalability and performance between MonoBehaviour and DOTS (ECS + Jobs + Burst)
- Simulate real-time combat with thousands of independent entities
- Showcase clean architecture, data-oriented design, and system separation
- Provide a real-time benchmark for architectural decision-making

---

## **Core Features**

### **DOTS Version**
- Simulates 10,000+ drones with 10x the performance
- Burst-compiled systems for movement, shooting, and collision detection
- Pure ECS logic using `ISystem`, `IJobEntity`, `LocalTransform`, and `EntityCommandBuffer`
- Clean data separation (movement, input, projectile, turret) and PlayerTag filtering
- Data-oriented layout for cache efficiency and scalable logic

### **GameObject Version**
- Identical simulation using MonoBehaviour
- Shared assets and logic for a fair comparison
- Demonstrates the scaling limitations of OOP in Unity
- Drops to ~4 FPS at 10,000 drones

---

## **Media**
![Performance comparison](https://i.imgur.com/r4z2DBv.jpeg)

> **Above:** Performance comparison between DOTS (ECS) and GameObject-based simulation at 10,000 drones.

---

## **Technologies**
- Unity ECS (Entities 1.0)
- Burst Compiler
- IJobEntity + ComponentData
- MonoBehaviour (comparison baseline)
- ScriptableObject tuning for flexible simulation

---

## **Developer Notes**
- Built with a “pure DOTS” mindset: logic lives entirely in systems
- Clean separation of logic, data, and tags
- DOTS implementation mirrors MonoBehaviour setup one-to-one
- Focused on system clarity and architecture — not visuals

---

## Contact

- **Name**: João Yakubets  
- **Email**: unduke2@tuta.io

---

