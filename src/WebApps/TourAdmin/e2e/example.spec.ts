import { expect, test } from '@playwright/test';

test.describe('basic application tests', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('/');
  });

  test('application has title set correctly', async ({ page }) => {
  // Expect a title "to contain" a substring.
    await expect(page).toHaveTitle('TourAdmin');
  });

  test('landing of the application page has a title', async ({ page }) => {
    await expect(page.getByText('Tour Admin', { exact: true })).toBeVisible();
  });

  test('landing of the application page has a welcome message', async ({ page }) => {
    await expect(page.getByText('Welcome to the Tour Admin application', { exact: true })).toBeVisible();
  });
});





