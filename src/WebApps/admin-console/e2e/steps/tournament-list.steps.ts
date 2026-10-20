import { expect } from '@playwright/test';
import { createBdd, DataTable } from 'playwright-bdd';
import { TournamentListPage } from '../drivers/tournament-list-page';

const { Given, When, Then } = createBdd();

const ACTIVE_TOURNAMENT_COUNT = 8;
const TOTAL_TOURNAMENT_COUNT = 10;

Given('I access the tour management page', async ({ page }) => {
  const tournamentListPage = new TournamentListPage(page);
  await tournamentListPage.NavigateToTournamentListPage();
});

Then('I should see a list of the active tournaments', async ({ page }) => {
  const tournamentListPage = new TournamentListPage(page);
  await tournamentListPage.VerifyTitleIsDisplayed();
  await tournamentListPage.VerifyTableIsDisplayed();
});

Then('The list should contain the correct set of tournaments', async ({ page }) => {
  const tournamentListPage = new TournamentListPage(page);
  expect(await tournamentListPage.GetTournamentCount()).toBe(ACTIVE_TOURNAMENT_COUNT);
});

Then('The option for showing inactive tournaments is {string}', async ({ page }, state: string) => {
  if (state !== 'on' && state !== 'off') {
    throw new Error(`Unsupported inactive tournaments option state: ${state}`);
  }
  const tournamentListPage = new TournamentListPage(page);
  expect(await tournamentListPage.GetShowInactiveOption()).toBe(state === 'on');
});

When('I opt to {string} inactive tournaments', async ({ page }, action: string) => {
  if (action !== 'view' && action !== 'hide') {
    throw new Error(`Unsupported inactive tournaments action: ${action}`);
  }
  const tournamentListPage = new TournamentListPage(page);
  await tournamentListPage.ToggleShowInactiveOption(action === 'view');
});

Given('I have inactive tournaments shown', async ({ page }) => {
  const tournamentListPage = new TournamentListPage(page);
  await tournamentListPage.ToggleShowInactiveOption(true);
  expect(await tournamentListPage.GetShowInactiveOption()).toBe(true);
});

Then('Inactive tournaments are included in the list', async ({ page }) => {
  const tournamentListPage = new TournamentListPage(page);
  expect(await tournamentListPage.GetTournamentCount()).toBe(TOTAL_TOURNAMENT_COUNT);
});

Then('Inactive tournaments are not shown', async ({ page }) => {
  const tournamentListPage = new TournamentListPage(page);
  expect(await tournamentListPage.GetTournamentCount()).toBe(ACTIVE_TOURNAMENT_COUNT);
});

Then('The properties of each tournament are displayed', async ({ page }, dataTable: DataTable) => {
  const tournamentListPage = new TournamentListPage(page);
  const expectedHeaders = dataTable.raw().map(([header]) => header);
  expect(await tournamentListPage.GetTableHeaderTitles()).toEqual(expectedHeaders);
});

When('I search the tournament list using a search text', async ({ page }) => {
  const tournamentListPage = new TournamentListPage(page);
  await tournamentListPage.SetSearchBoxValue('open');
});

Then('Only the tournaments whose title contains the search text are shown', async ({ page }) => {
  const tournamentListPage = new TournamentListPage(page);
  const titles = await tournamentListPage.GetTournamentTitles();
  expect(titles).toHaveLength(4);
  expect(titles.every((title) => /open/i.test(title))).toBe(true);
});

Given('I have searched the tournament list using a search text', async ({ page }) => {
  const tournamentListPage = new TournamentListPage(page);
  await tournamentListPage.SetSearchBoxValue('OpEn');
});

When('I clear the search text', async ({ page }) => {
  const tournamentListPage = new TournamentListPage(page);
  await tournamentListPage.ClearSearchBox();
});

Then('All tournaments are shown', async ({ page }) => {
  const tournamentListPage = new TournamentListPage(page);
  expect(await tournamentListPage.GetTournamentCount()).toBe(ACTIVE_TOURNAMENT_COUNT);
});

When('I search the tournament list using a search text with different casing', async ({ page }) => {
  const tournamentListPage = new TournamentListPage(page);
  await tournamentListPage.SetSearchBoxValue('OpEn');
});
