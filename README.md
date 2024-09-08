# Static WebServer Project

## Description

This is a console application developed in C# that leverages `HttpListener` to handle incoming requests. It parses text to serve files and images using a `Router` class. Currently, text parsing is limited to absolute paths (filenames) and does not handle any attributes. Additionally, the request method is not parsed.

## Purpose

This project is part of my portfolio. While working on ASP.NET applications, I realized it would be beneficial to delve deeper into web server development by creating a simple web server from scratch.

## Target Audience

This project is aimed at job recruiters who are interested in exploring my general projects and interests.

## Getting Started / Running the Application

1. Clone the repository to your local machine.
2. The application detects its directory location for the executable file and listens on port [http://localhost:8080/](http://localhost:8080/).
3. Add HTML and image files directly to the `Views` folder.
4. The server will automatically look for these files and serve them when requested in the browser.

### Supported File Extensions

- `.ico`
- `.png`
- `.jpg`
- `.gif`
- `.bmp`
- `.html`
- `.css`
- `.js`

## Work in Progress

- Session system

Feel free to contribute or provide feedback on the project!
