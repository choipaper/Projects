#include "Player.h"

const float Player::PLAYER_VEL = 10.f;

Player::Player(const int x, const int y)
	: mPos{(float)x ,(float)y}
	, mVel{0,0}
{
	mCollider.UpperBound = { PLAYER_WIDTH, mPos.Y - PLAYER_HEIGHT / 2 };
	mCollider.LowerBound = { mPos.X - PLAYER_WIDTH / 2 , PLAYER_HEIGHT };
	mGameObj.Type = eType::PLAYER;
}

void Player::HandleEvent(SDL_Event& e)
{
	// If a key was pressed
	// a key reapeat is enabled by default and if press and hold -> multiple presses(repeat)
	if (e.type == SDL_KEYDOWN && e.key.repeat == 0)
	{
		// Adjust the velocity
		switch (e.key.keysym.sym)
		{
			case SDLK_LEFT: mVel.X -= PLAYER_VEL; break;
			case SDLK_RIGHT: mVel.X += PLAYER_VEL; break;
		}
	}
	// If a key was released
	else if (e.type == SDL_KEYUP && e.key.repeat == 0)
	{
		// Adjust the velocity
		switch (e.key.keysym.sym)
		{
			case SDLK_LEFT: mVel.X += PLAYER_VEL; break;
			case SDLK_RIGHT: mVel.X -= PLAYER_VEL; break;
		}
	}
}

// Move Player left or right only
void Player::MovePlayer(const int screenWidth)
{
	// Move the player left or right
	mPos.X += mVel.X;
	mCollider.UpdateBounds(mPos.X,mPos.Y, PLAYER_WIDTH, PLAYER_HEIGHT);

	// If the player hit the window left or right
	if ((mPos.X < 0) || (mPos.X + PLAYER_WIDTH > screenWidth))
	{
		// Move back
		mPos.X -= mVel.X;
		mCollider.UpdateBounds(mPos.X, mPos.Y, PLAYER_WIDTH, PLAYER_HEIGHT);
	}
	
}

// Draw a Black box(a bar shape)
void Player::RenderPlayer(SDL_Renderer* renderer)
{
	// Draw Collision box
	SDL_Rect box = { mCollider.LowerBound.X + ((mCollider.UpperBound.X - mCollider.LowerBound.X) / 2),
		mCollider.UpperBound.Y + ((mCollider.LowerBound.Y - mCollider.UpperBound.Y) / 2),
		mCollider.UpperBound.X - mCollider.LowerBound.X,
		mCollider.LowerBound.Y - mCollider.UpperBound.Y };
	// Draw player
	SDL_Rect player = { mPos.X, mPos.Y, PLAYER_WIDTH, PLAYER_HEIGHT };
	SDL_SetRenderDrawColor(renderer, 0x00, 0x00, 0x00, 0xFF);
	SDL_RenderFillRect(renderer, &player);
	SDL_SetRenderDrawColor(renderer, 0xFF, 0x00, 0x00, 0xFF);
	SDL_RenderDrawRect(renderer, &box);
}

Vec2D Player::GetPos() const
{
	return {mPos.X, mPos.Y};
}

BoxCollider2D Player::GetCollider() const
{
	return mCollider;
}
