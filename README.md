# Description:
This solution demonstrates a real-time communication system using SignalR with a WPF SignalR Hub and two clients:

A WPF Client using .NET Framework 4.7.2.
An MVC Client using .NET Core 8.0.
The solution enables bi-directional communication between the server (SignalR Hub) and the two clients, showcasing the capabilities of SignalR in a cross-platform setup.

# Solution Structure:
WpfSignalRHub

Type: WPF Application
Purpose:
Acts as the central hub for the SignalR communication. The hub is hosted within this application and manages messages exchanged between connected clients.
Key Features:
Hosts a SignalR hub at http://localhost:8080.
Allows connections from multiple clients.
WpfSignalRClient

Type: WPF Application (.NET Framework 4.7.2)
# Purpose:
A client application that connects to the SignalR hub to send and receive messages in real-time.
Key Features:
Connects to the SignalR hub hosted by WpfSignalRHub.
Displays messages sent by other clients in real-time.
Allows users to send new messages.
MvcSignalRClient

Type: ASP.NET Core 8.0 MVC Application
# Purpose:
A web-based client application that connects to the SignalR hub for real-time communication.
Key Features:
Connects to the SignalR hub hosted by WpfSignalRHub.
Provides a web-based user interface for sending and receiving messages.
Demonstrates how SignalR can be integrated into an ASP.NET Core MVC application.
Features:
Real-time messaging between clients via the SignalR hub.
Cross-platform communication between a desktop WPF client and a web-based MVC client.
SignalR hub hosted within a WPF application, demonstrating a unique hosting setup.
Simple and intuitive user interfaces for both clients to send and receive messages.
Technologies Used:
WPF Application: .NET Framework 4.7.2
ASP.NET Core MVC Application: .NET Core 8.0
SignalR: For real-time communication between the server and clients.
C#: Core programming language used in the project.
How to Run the Solution:
Start the SignalR Hub:

Open the WpfSignalRHub project and run it.
Ensure the hub is accessible at http://localhost:8080.
Start the MVC Client:

Open the MvcSignalRClient project.
Run the project in Visual Studio.
Navigate to the Chat page (https://localhost:7036/Chat/Index) in your browser.
Start the WPF Client:

Open the WpfSignalRClient project and run it.
Use the client interface to send and receive messages.
Test the Communication:

Send messages from either client.
Verify that messages are displayed in real-time on both the WPF and MVC clients.
Prerequisites:
Visual Studio 2022 or later (for .NET Core 8.0 support).
.NET Framework 4.7.2 installed for the WPF projects.
Ensure that the default ports (8080 for SignalR Hub, 7036 for MVC Client) are not blocked or in use.

# Use Cases:
This project serves as a foundational example for building:

Chat applications with real-time messaging.
Cross-platform communication solutions.
SignalR-based notification systems.
