#ifndef PLAYER_H
#define PLAYER_H

/*
For linux develop
#include <SDL2/SDL.h>
#include <SDL2/SDL_image.h>
*/
#include <SDL.h>
#include <iostream>
// Include engine
#include "../../Engine/Paper2d.h"

class Player
{
public:
	// The dimensions of the dot
	static const int PLAYER_WIDTH = 50;
	static const int PLAYER_HEIGHT = 10;

	// Maximum axis velocity of the dot
	static const float PLAYER_VEL;
	// Initialize player
	Player(const int x, const int y);
	void HandleEvent(SDL_Event& e);
	void MovePlayer(const int screenWdith);
	void RenderPlayer(SDL_Renderer* renderer);

	// Getter
	Vec2D GetPos() const;
	BoxCollider2D GetCollider() const;
	GameObeject GetType() const;

private:
	Vec2D mPos;
	Vec2D mVel;
	BoxCollider2D mCollider;
	GameObeject mGameObj;

};

#endif //PLAYER_H
