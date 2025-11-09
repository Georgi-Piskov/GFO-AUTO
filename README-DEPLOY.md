# Deploy Options (GitHub → One‑click)

This app is an ASP.NET Core 8 Razor Pages server, so it needs a runtime that can host a long‑running process. Netlify’s static hosting cannot run ASP.NET Core servers. Use one of these free PaaS options instead:

## Render (recommended, free tier)

Repo already contains `render.yaml` and `NoActivityFiler/Dockerfile`.

Steps:

1) Create a Render account and link GitHub.
2) Click “New +” → “Blueprint” and select this repo (Render finds `render.yaml`).
3) In the created Web Service, set environment variable `N8N_WEBHOOK_URL` to your webhook URL.
4) Deploy. Render builds the Docker image and runs it on port 8080.

## Fly.io (already wired)

Repo contains `fly.toml`, Dockerfile, and a GitHub Actions workflow `.github/workflows/fly-deploy.yml`.

Quick setup:
- In GitHub → Settings → Actions → Secrets and variables:
  - Variables: `FLY_APP_NAME` = your app name
  - Secrets: `FLY_API_TOKEN` (from `fly auth token`), optional `N8N_WEBHOOK_URL`
- Push to `main` or run the workflow manually.

## Koyeb/Railway

Both detect the Dockerfile automatically when you connect the repo. Set `N8N_WEBHOOK_URL` in their dashboards. No extra files required.

