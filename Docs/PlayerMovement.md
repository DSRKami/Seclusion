# PlayerMovement System

## Purpose
Enables smooth 8-Directional 2D to-down movement. Designed for expandability (sprint, dash, etc.)

## Components
- 'InputReader.cs' - Captures and normalises input
- 'PlayerMovement.cs' - Applies movement to Rigidbody2D

## Features
- Normalised 2D movement (WASD or Arrow Keys)
- Movement speed defined externally
- Movement is framerate-independent (FixedUpdate)
- Modular