
#include "Ball.h"

const float Ball::BALL_SIZE = 5.f;
const float Ball::BALL_VEL = 1.8f;

Ball::Ball(const int x, const int y)
	: mPos{(float)x, (float)y}
	, mVel{	BALL_VEL, BALL_VEL }
	, mbHit(false)
{
	// Set initial direction (to NE)
	mDir.X = 1;
	mDir.Y = -1;

	// Set box collider
	mCollider.UpperBound = { BALL_SIZE, mPos.Y - BALL_SIZE / 2 };
	mCollider.LowerBound = { mPos.X - BALL_SIZE / 2 , BALL_SIZE };
	
	mGameObj.Type = eType::BALL;
	mOtherObject.Type = eType::UNDEFINED;
}

void Ball::MoveBall(double deltaTime)
{
	// Collision detection
	// Change direction ( simply using law of reflection (since no fraction))
	// constant velocity 
	// if hit anything
	if (mbHit)
	{
		mbHit = false;
		/**********************************************************************************
		*								DEBUG											  *
		**********************************************************************************/
		//std::cout << "After hit: " << mDir.X << ", " << mDir.Y << std::endl;
	}
	// Test: wall collision 
	/*if (mPos.X <= screenWidth || mPos.X >= screenWidth || mPos.Y <= screenHeight || mPos.Y >= screenHeight)
	{
		mDir.X *= -1;
		mDir.Y *= -1;
	}*/

	// Calculate velocity: velocity = V0 + a*t(deltaTime since no friction)
	mVel.X = deltaTime * mVel.X;
	mVel.Y = deltaTime * mVel.Y;

	// Move ball
	mPos.X += mVel.X * mDir.X;
	mPos.Y += mVel.Y * mDir.Y;
	mCollider.UpdateBounds(mPos.X, mPos.Y, BALL_SIZE, BALL_SIZE);

	//DrawBall(renderer);
	/**********************************************************************************
	*								DEBUG											  *
	**********************************************************************************/
	//std::cout << "ball pos: " << mPos.X << "," << mPos.Y << std::endl;
	//std::cout << "ball collider pos: " << mCollider.LowerBound.X << "," << mCollider.LowerBound.Y << std::endl;
}

void Ball::DrawBall(SDL_Renderer* renderer)
{
	// Draw collider box
	SDL_Rect box = { mCollider.LowerBound.X + ((mCollider.UpperBound.X - mCollider.LowerBound.X) / 2),
		mCollider.UpperBound.Y + ((mCollider.LowerBound.Y - mCollider.UpperBound.Y) / 2),
		mCollider.UpperBound.X - mCollider.LowerBound.X,
		mCollider.LowerBound.Y - mCollider.UpperBound.Y };
	// Draw ball 
	SDL_Rect ballImag = { mPos.X, mPos.Y, BALL_SIZE, BALL_SIZE };

	SDL_SetRenderDrawColor(renderer, 0x00, 0x00, 0x00, 0xFF);
	SDL_RenderFillRect(renderer, &ballImag);
	SDL_SetRenderDrawColor(renderer, 0x00, 0x00, 0xFF, 0xFF);
	SDL_RenderDrawRect(renderer, &box);
}


void Ball::SetHit(bool signal, int caseNum)
{
	mbHit = signal;
	switch (caseNum)
	{
		case 1: 
			mDir.Y *= -1; 
			break;			
		case 2: 
			mDir.X *= -1; 
			break;
		case 3: 
			mDir.Y *= -1;
			break;
		case 4: 
			mDir.X *= -1; 
			break;
	}
}

BoxCollider2D Ball::GetCollider() const
{
	return mCollider;
}
