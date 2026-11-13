import { env } from 'node:process';
import { expect } from '@playwright/test';
import { createBdd } from 'playwright-bdd';
import { test } from '../fixtures';

const { Given, When, Then } = createBdd(test);
const baseUrl = env['ADMIN_CONSOLE_URL'] ?? 'http://localhost:4200';

Given('I have changed a tournament detail', async ({ page }) => {
  await page.route('https://localhost:7288/tournaments', (route) =>
    route.fulfill({ json: [] }),
  );
  await page.goto(`${baseUrl}/tour/create-tournament`);
  await page.getByLabel('Tournament', { exact: true }).fill('Open');
});

When('I cancel tournament creation', async ({ page }) => {
  await page.getByRole('button', { name: 'Cancel' }).click();
});

When('I choose not to discard my changes', async ({ page }) => {
  await page.getByRole('dialog').getByRole('button', { name: 'No' }).click();
});

Then('I remain on the tournament form with my changes', async ({ page }) => {
  await expect(page.getByRole('dialog')).toBeHidden();
  await expect(page.getByLabel('Tournament', { exact: true })).toHaveValue('Open');
});

When('I choose to discard my changes', async ({ page }) => {
  await page.getByRole('dialog').getByRole('button', { name: 'Yes' }).click();
});

Then('I return to the tournament list', async ({ page }) => {
  await expect(page).toHaveURL(`${baseUrl}/tour`);
});
