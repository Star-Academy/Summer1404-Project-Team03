🚀 ETL Frontend

This project is an Angular frontend application that can be run in two modes:

Development mode → live reload with Angular CLI (ng serve)

Production mode → built Angular app served with nginx

📦 Requirements

Docker
 (20+ recommended)

Docker Compose

🛠 Project Structure
etl_frontend/
├── src/...
├── package.json
├── angular.json
├── nginx.conf
├── Dockerfile             # Production build
├── Dockerfile.dev         # Development build
├── docker-compose.yml     # Production compose
└── docker-compose.dev.yml # Development compose

🔧 Development Mode (Hot Reload)

Run Angular with live reload (ng serve) inside Docker.
Any local code changes will automatically rebuild inside the container.

docker compose -f docker-compose.dev.yml up --build


App available at: http://localhost:4200

Local code is mounted into the container.

Uses Dockerfile.dev.

🚀 Production Mode (nginx)

Build a static Angular production bundle and serve it via nginx.

docker compose up --build


App available at: http://localhost

Runs the built Angular app from /dist/etl_frontend/browser.

Uses Dockerfile and nginx.conf.

⚙️ Notes

Make sure package.json has this script for dev mode:

"scripts": {
  "start": "ng serve --host 0.0.0.0 --poll 2000"
}


Your nginx.conf should handle Angular SPA routing (redirect unknown routes to index.html):

server {
  listen 80;

  root /usr/share/nginx/html;
  index index.html;

  location / {
    try_files $uri $uri/ /index.html;
  }
}

🧹 Cleanup

Stop and remove containers:

docker compose down
docker compose -f docker-compose.dev.yml down

✅ Summary

Production → docker compose up --build

Development → docker compose -f docker-compose.dev.yml up --build