FROM node:20 AS build

WORKDIR /app

COPY package*.json ./

# Install dependencies
RUN npm ci

# Copy source code
COPY . .

# Run lint (optional, will fail if lint errors)
RUN npm run lint --if-present

# Run tests (fail on errors, headless mode)
RUN npm test -- --watch=false --browsers=ChromeHeadless

# Build Angular app (production build)
RUN npm run build --if-present

# Stage 2: Serve Angular with Nginx
FROM nginx:alpine

# Copy Angular dist to Nginx
COPY --from=build /app/dist /usr/share/nginx/html

# Copy default nginx config (optional override)
# COPY nginx.conf /etc/nginx/nginx.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
