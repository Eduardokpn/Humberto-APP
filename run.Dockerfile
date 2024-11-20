FROM gcr.io/buildpacks/gcp/run
USER root
RUN apt-get update && apt-get install -y --no-install-recommends \
  imagemagick && \
  apt-get clean && \
  rm -rf /var/lib/apt/lists/*
USER 33:33