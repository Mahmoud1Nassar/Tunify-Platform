# Tunify Platform

## Introduction

Tunify Platform is a web application that allows users to manage playlists, songs, and subscriptions. This platform supports user accounts, various types of subscriptions, and detailed metadata for artists, albums, and songs.

## Entity-Relationship Diagram (ERD)

![ERD](/Tunify-Platform/ERD.png)

## Models and Relationships

- **User**: Represents a user of the platform.
- **Subscription**: Represents different subscription plans available.
- **Playlist**: Represents a user's playlist containing multiple songs.
- **Song**: Represents a song with details about its artist and album.
- **Artist**: Represents an artist who has released songs and albums.
- **Album**: Represents an album that contains multiple songs.
- **PlaylistSongs**: Represents the many-to-many relationship between playlists and songs.

## Seed Data
- **Users**: One user with ID 1, name "John Doe", and email "john.doe@example.com".
- **Artists**: One artist with ID 1, name "Artist A".
- **Albums**: One album with ID 1, title "Album A", and artist ID 1.
- **Songs**: One song with ID 1, title "Song A", artist ID 1, album ID 1, and duration of 3 minutes and 45 seconds.
- **Playlists**: One playlist with ID 1, name "My Playlist", and user ID 1.
- **PlaylistSongs**: One entry linking playlist ID 1 and song ID 1.
- **Subscriptions**: Two subscription plans - "Free" with price $0.00 and "Premium" with price $9.99.