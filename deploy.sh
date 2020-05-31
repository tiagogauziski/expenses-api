#!/bin/bash
set -ev
set -x

TAG=$TRAVIS_TAG
BRANCH=$TRAVIS_BRANCH

# Remove a leading v from the major version number (e.g. if the tag was v1.0.0)
IFS='.' read -r -a tag_array <<< "$TAG"
MAJOR="${tag_array[0]//v}"
MINOR=${tag_array[1]}
BUILD=${tag_array[2]}
SEMVER="$MAJOR.$MINOR.$BUILD"

# Build the Docker images
docker build -f src/Expenses.API/Dockerfile . -t $DOCKER_REPOSITORY

# Login to Docker Hub and upload images
docker login $DOCKER_REGISTRY --username $DOCKER_LOGIN --password $DOCKER_PASSWORD

if [ -z "$TAG" ]
then
    echo "Building from branch $BRANCH"

    docker tag $DOCKER_REPOSITORY $DOCKER_REPOSITORY:$BRANCH

    docker push $DOCKER_REPOSITORY:$BRANCH
else
    echo "Building from tag $TAG"

    docker tag $DOCKER_REPOSITORY $DOCKER_REPOSITORY:latest
    docker tag $DOCKER_REPOSITORY $DOCKER_REPOSITORY:$SEMVER

    docker push $DOCKER_REPOSITORY:$SEMVER
    docker push $DOCKER_REPOSITORY:latest
fi
