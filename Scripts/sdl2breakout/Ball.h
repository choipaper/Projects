#pragma once
/*
For linux develop
#include <SDL2/SDL.h>
*/
#include <SDL.h>
#include <iostream>
// Include engine
#include "../../Engine/Paper2d.h"

class Ball
{
public:
	// Size of ball
	static const float BALL_SIZE;
	// Initial velocity of ball
	static const float BALL_VEL;

	// Initialize ball
	Ball(const int x, const int y);
	
	// Movement
	void MoveBall(double detaTime);
	
	// Render 
	// draw ball
	void DrawBall(SDL_Renderer* renderer);
	// TODO:draw box collision

	// Check Collisions 
	void CheckCollision(BoxCollider2D& other);

	// Set hit signal and which gameobject is hit
	//void SetHit(bool signal);//, GameObeject type);
	void SetHit(bool signal, int caseNum); //Test case

	// Get Collider
	BoxCollider2D GetCollider() const;

private:
	Vec2D mPos;
	Vec2D mVel;
	// mDir can be changed since it only need sign of number
	Vec2D mDir;
	BoxCollider2D mCollider;
	bool mbHit;
	GameObeject mGameObj;
	GameObeject mOtherObject;

};

