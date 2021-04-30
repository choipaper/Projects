/**
 * 
 */

/*
For linux develop
#include <SDL2/SDL.h>
#include <SDL2/SDL_image.h>
*/
//#define NDEBUG

#include <SDL.h>
#include <SDL_image.h>

#include <iostream>
#include "Player.h"
#include "Ball.h"


const int SCREEN_WIDTH = 680;
const int SCREEN_HEIGHT = 480;

const int TOTAL_BRICKS = 55;
const int BRICK_WIDHT = 50;
const int BRICK_HEIGHT = 10;

bool Init();
bool LoadImage();
void CloseSDL();

void DrawBricks(int index);
bool CollideBricks(Ball& ball);

SDL_Window* gWindow = nullptr;
SDL_Renderer* gRenderer = nullptr;

SDL_Rect gBricks[TOTAL_BRICKS];

int main(int argc, char* args[])
{

	if (!Init())
	{
		std::cerr << "Failed to initialize!" << std::endl;
	}
	else
	{
		bool quit = 0;
		SDL_Event e;

		// Time clock setup
		//Uint64 now = SDL_GetPerformanceCounter();
		//Uint64 last = 0;
		Uint64 start = SDL_GetTicks();
		double deltaTime = 0;

		//
		Player player = {SCREEN_WIDTH/2, SCREEN_HEIGHT - 50};

		// Ball
		Ball ball(SCREEN_WIDTH / 3, SCREEN_HEIGHT - 50);
		while (!quit)
		{
			while (SDL_PollEvent(&e) != 0)
			{
				if (e.type == SDL_QUIT)
				{
					quit = true;
				}
				
				// Handle input key for player
				player.HandleEvent(e);
			}

			// Timer setting
			//last = now;
			//now = SDL_GetPerformanceCounter();
			//deltaTime = (double)((now - last) * 1000 / (double)SDL_GetPerformanceFrequency());
			deltaTime = SDL_GetTicks() - start;
			/**********************************************************************************
			*								DEBUG											  *
			**********************************************************************************/
			//std::cout << "deltaTime: " << deltaTime << std::endl;

			// handle player movement
			player.MovePlayer(SCREEN_WIDTH);
			
			// Move a ball
			// Test check ball collision with window

			if(ball.GetCollider().UpperBound.Y <= 0)
			{
				//std::cout << "case 1" << std::endl;
				ball.SetHit(true,1);
			}
			if(ball.GetCollider().UpperBound.X >= SCREEN_WIDTH) 
			{
				ball.SetHit(true,2);
				//std::cout << "case 2" << std::endl;
			}
			// collision on bottom of collider 
			// It should be game over 
			// TODO: change it to Game over
			if (ball.GetCollider().LowerBound.Y >= SCREEN_HEIGHT )
			{
				ball.SetHit(true, 3);
				//std::cout << "case 3" << std::endl;
			}
			if (ball.GetCollider().LowerBound.X <= 0)
			{
				ball.SetHit(true, 4);
				//std::cout << "case 4" << std::endl;
			}
			// collision with player
			// !!!!Current Error: when ball goes down to SE direction -> detection ignored
			// and sometimes opposite direction detection ignored too 
			
			// fix try01
			if (ball.GetCollider().IsCollided(player.GetCollider()))
			{
				std::cout << "ball collided with player" << std::endl;
				// testing 
				ball.SetHit(true, 3);
			}
			// collide with bricks
			/*if (CollideBricks(ball))
			{
				std::cout << "Inside of if loop" << std::endl;
			}*/

			ball.MoveBall(1);
			
			

			// Clear Screen
			SDL_SetRenderDrawColor(gRenderer, 0xFF, 0xFF, 0xFF, 0xFF);
			SDL_RenderClear(gRenderer);
			
			DrawBricks(TOTAL_BRICKS);

			// Render player
			player.RenderPlayer(gRenderer);

			ball.DrawBall(gRenderer);

			SDL_RenderPresent(gRenderer);
			//Debug
			//std::cout << "Rendered" << std::endl;
		}
	}
	CloseSDL();
	return 0;
}

bool Init()
{
	bool success = true;

	if (SDL_Init(SDL_INIT_VIDEO) < 0)
	{
		std::cerr << "SDL could not be initialized! SDL_Error: " << SDL_GetError() << std::endl;
		success = false;
	}
	else
	{
		gWindow = SDL_CreateWindow("Breakout!", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, SCREEN_WIDTH, SCREEN_HEIGHT, SDL_WINDOW_SHOWN);
		if (gWindow == nullptr)
		{
			std::cerr << "Window could not be created! SDL_Error: " << SDL_GetError() << std::endl;
			success = false;
		}
		else
		{
			gRenderer = SDL_CreateRenderer(gWindow, -1, SDL_RENDERER_ACCELERATED | SDL_RENDERER_PRESENTVSYNC);
			if (gRenderer == nullptr)
			{
				std::cerr << "Renderer could not be created! SDL_Error: " << SDL_GetError() << std::endl;
				success = false;
			}
		}
	}

	return success;
}

// PROBLEM: need to add box collider for bricks 
void DrawBricks(int index)
{
	const int startPointX = 45;
	const int startPointY = 20;
	const int spaceBTWBricks = 2;
	const int totalBrksPerLine = 11;
	for (int i = 0; i < TOTAL_BRICKS; i++)
	{
		if (i == index)
		{
			gBricks[i].x = 0;
			gBricks[i].y = 0;
			gBricks[i].w = 0;
			gBricks[i].h = 0;
		}
		else
		{
			gBricks[i].x = startPointX + (spaceBTWBricks + BRICK_WIDHT) * (i % totalBrksPerLine);
			gBricks[i].y = startPointY + (spaceBTWBricks + BRICK_HEIGHT) * (i / totalBrksPerLine);
			gBricks[i].w = BRICK_WIDHT;
			gBricks[i].h = BRICK_HEIGHT;
		}
		
		// set colors line by line (blk, R, G, B, blk)
		if (i / totalBrksPerLine == 0 || i / totalBrksPerLine == 4)
		{
			SDL_SetRenderDrawColor(gRenderer, 0x00, 0x00, 0x00, 0xFF);
		}
		if (i / totalBrksPerLine == 1)
		{
			SDL_SetRenderDrawColor(gRenderer, 0xFF, 0x00, 0x00, 0xFF);
		}
		if (i / totalBrksPerLine == 2)
		{
			SDL_SetRenderDrawColor(gRenderer, 0x00, 0xFF, 0x00, 0xFF);
		}
		if (i / totalBrksPerLine == 3)
		{
			SDL_SetRenderDrawColor(gRenderer, 0x00, 0x00, 0xFF, 0xFF);
		}
		if (i != index)
		{
			SDL_RenderFillRect(gRenderer, &gBricks[i]);
		}
		
	}
	
}

bool CollideBricks(Ball& ball)
{
	bool collided = false;
	// Temp variables
	const int startPointX = 45;
	const int startPointY = 20;
	const int spaceBTWBricks = 2;
	const int totalBrksPerLine = 11;

	// if ball enters bricks area
	if ((ball.GetCollider().LowerBound.X >= startPointX && ball.GetCollider().UpperBound.X <= 615) && (ball.GetCollider().UpperBound.Y >= startPointY && ball.GetCollider().LowerBound.Y <= 78))
	{
		#ifdef NDEBUG
		std::cout << "Ball enters bricks area" << std::endl;
		#endif // !NDEBUG

		// loop through bricks 
		// find if anybricks collide with ball
		for (int i = 0; i < TOTAL_BRICKS; i++)
		{
			if(ball.GetCollider().UpperBound.Y <= gBricks[i].y + gBricks[i].h/2)
			{
				#ifdef NDEBUG
				std::cout << "Hit bricks: " << i << std::endl;
				#endif // !NDEBUG

				ball.SetHit(true,1);
				collided = true;
				// delete the brick
				// for now send index to DrawBricks() to re-draw bricks 
				// when re-drawing bricks, skips the brick that was hit
				DrawBricks(i);

			}
			if(ball.GetCollider().UpperBound.X >= SCREEN_WIDTH) 
			{


				ball.SetHit(true,2);
				#ifdef NDEBUG
				std::cout << "Hit bricks: " << i << std::endl;
				#endif // !NDEBUG
				collided = true;
			}
			if (ball.GetCollider().LowerBound.Y >= SCREEN_HEIGHT )
			{
				ball.SetHit(true, 3);
				#ifdef NDEBUG
				std::cout << "Hit bricks: " << i << std::endl;
				#endif // !NDEBUG

				
				collided = true;
			}
			if (ball.GetCollider().LowerBound.X <= 0)
			{
				ball.SetHit(true, 4);
				#ifdef NDEBUG
				std::cout << "Hit bricks: " << i << std::endl;
				#endif // !NDEBUG
				collided = true;
			}
		}
	}

	return collided;
}

void CloseSDL()
{
	SDL_DestroyRenderer(gRenderer);
	gRenderer = nullptr;
	SDL_DestroyWindow(gWindow);
	gWindow = nullptr;

	SDL_Quit();
}